# DEPLOY.md — Roadlab v17 production deploy runbook

Step-by-step for cutting `upgrade/v17` over to live production. Treat as
the canonical sequence — every step has been chosen because skipping or
reordering it caused pain during local rehearsal.

The current production target is the Azure App Service that serves
`https://roadlab.co.za/` (still on v13 at the time of writing). The
Azure DevOps push URL on this repo's `origin` remote is intentionally
disabled — that pipeline still deploys v13. The v17 deploy will be
performed manually until the pipeline is rewritten (`fixes.md §7.9`).

---

## Pre-flight (1-2 days before)

1. **Schedule a maintenance window** — ~30 min of read-only / partial
   downtime on `roadlab.co.za`.
2. **Anton coordination required for:**
   - Rotating the Azure SQL `interon` user password.
   - Rotating the SMTP `webserver@roadlab.co.za` password.
   - Generating a new uSync Publisher AppKey.
   - Switching production to Azure Key Vault references (or env vars)
     for all three so the repo can stop carrying plaintext secrets.
3. **Take a fresh production backup** of:
   - Azure SQL DB (point-in-time restore is on, but a manual snapshot
     just before the cutover is cheap insurance).
   - Live `wwwroot/media/` folder on the App Service.
4. **Walk through this runbook locally one more time** against a
   restored copy of the prod DB if at all possible.

---

## Cutover day

### 1. Freeze the v13 site
Put the existing site into read-only mode by disabling backoffice
logins (or swap to the App Service "maintenance" slot if one is set
up). Editors must not save during the deploy.

### 2. Force-rewrite history to scrub secrets
The repo currently carries plaintext credentials in `appsettings.json`
that pre-date the rotation. After Anton has rotated everything, force-
rewrite history on the `Slaplap/roadlab17` GitHub remote so the old
values don't ship with the source. (Anton handles the GitHub credential
side.)

### 3. Deploy code to prod
Publish `upgrade/v17` to the production App Service slot. This pushes
the .NET 10 / Umbraco 17.4.1 binaries — but the prod DB is still
v13's schema until step 4.

### 4. Umbraco upgrade migration
Boot the v17 App Service slot against the v13 DB. Umbraco's built-in
upgrade migrations will run automatically. Wait for the boot log
"Database upgrade complete" before proceeding.

### 5. Data conversion script
Run `tools/Convert-V13-To-V17-Data.ps1` against the production DB.
This script is idempotent. It converts:
- Nested Content → Block List values
- MediaPicker v1 (UDI string) → MediaPicker3 (JSON array)

### 6. uSync schema + content sync
In the production backoffice (or via the CLI if wired up):
- **Settings → uSync → Import (Force)**, run **twice**.
  - First run: applies the schema and most content. May raise the
    known "Cannot save a non-current version" exception in the
    second-pass save — that's the documented #1b race; first-pass
    writes still commit.
  - Second run: any items that failed second-pass on run 1 succeed
    on run 2 because the entities are stable.
- If a third run shows zero "non-current version" errors, the import
  is settled.

### 7. Media bulk-copy
Copy the live media from the v13 staging Azure storage account into
prod's v17 storage:
- Source: the existing roadlabstaging.azurewebsites.net media folder
  (~1,386 files, ~1.4 GB).
- Destination: the prod App Service `wwwroot/media/` (or whatever
  storage backing Umbraco's media filesystem uses on prod).
- `azcopy sync` is the recommended tool. Run it twice and confirm the
  second run reports zero deltas.

### 8. Disable the dev media proxy
In production `appsettings.json` (or `appsettings.Production.json`):
```json
"Custom": {
  "LoadMediaFromStaging": false
}
```
This stops the `MediaFileMiddleware` from reaching back to staging.
On prod it should always be `false`.

### 9. Verify `appsettings.json` is clean
Confirm the prod settings file references Key Vault / env vars for:
- `ConnectionStrings:umbracoDbDSN`
- `EmailSettings:SmtpSettings:Password`
- `uSync:Publisher:Settings:AppKey`
- `AnthropicApiKey`

No plaintext secrets in source.

### 10. Cache rebuild
Trigger a content cache rebuild from Settings → Published Status →
"Reload memory cache". Confirm no errors in the log.

### 11. Smoke test
Hit each of these on prod and confirm 200 + correct rendering:
- `/`
- `/about/`
- `/branches/` (listing) and one specific branch like `/branches/durban/`
- `/services/materials-testing/asphalt/` (a Services2-template page)
- `/services/materials-testing/aggregate/` (a Services template page)
- `/blog/` and any one article
- `/head-office/` — submit the contact form with a real address;
  confirm an email lands.
- `/careers/` (or wherever Vacancies surfaces) — submit the vacancy
  form with a small PDF; confirm an email lands.
- `/umbraco` — backoffice loads, can edit and save a piece of content.
- View source on the homepage — confirm `<link rel="canonical">`
  and the `LocalBusiness` / `BreadcrumbList` JSON-LD render.
- View response headers — confirm `X-Content-Type-Options: nosniff`,
  `X-Frame-Options: SAMEORIGIN`, `Referrer-Policy: strict-origin-when-cross-origin`,
  and `Strict-Transport-Security: max-age=...` (HSTS).

### 12. Re-enable Azure DevOps push when ready
Once the v17 deploy is verified, the `origin` push URL can be
restored:
```
git remote set-url --push origin "<azure-devops-PAT-URL>"
```
But the `azure-pipelines.yml` itself needs a rewrite first — the
existing pipeline targets the Xamarin / .NET 8 / Umbraco 13 build
(see `fixes.md §7.9`). Wiring that up is its own follow-up.

---

## Rollback plan

If anything in steps 4-11 goes wrong:

1. Swap the App Service back to the v13 slot (or redeploy v13 from the
   `staging` branch).
2. Restore the SQL DB from the snapshot taken in pre-flight step 3.
3. Restore media from the snapshot if step 7 was destructive.
4. Re-enable editor logins.

Rollback is a 5-minute operation if the slot swap is set up.

---

## After the deploy lands

- Disable the v9 reference content in `Interon.Roadlab.Web.v17/uSync/v9/`
  unless the conversion scripts still need it for reruns. (Or delete
  outright once v9 is no longer the source-of-truth for anything.)
- Schedule the deferred items in `TODO.md` (Grid → BlockGrid, image
  re-encoding, CSP, Bootstrap 5, etc.) as their own work sessions.
- Rotate the `rfk_*` ai-auditor API key that previously sat in HTML
  before commit `7ca371e0` removed it — the key was exposed in
  public source for the lifetime of those commits.
