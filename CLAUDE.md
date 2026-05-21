# CLAUDE.md

Guidance for Claude Code (claude.ai/code) when working on this repository.

## Current state (read this first)

This codebase was migrated from **Umbraco 13 / .NET 8** to **Umbraco 17.4.1 / .NET 10** on the `upgrade/v17` branch. The migration is functionally complete locally — the site boots, content renders, and the v13 staging is mirrored. Pre-deploy work is still outstanding (see `TODO.md`).

- **Live staging:** `https://roadlabstaging.azurewebsites.net/` — still v13. **DO NOT** push to the Azure DevOps remote; its push URL is intentionally set to `DISABLED_NO_PUSH_TO_AZURE` so we don't auto-deploy unfinished v17 work. Use `git push github upgrade/v17` to push to the GitHub mirror (`Slaplap/roadlab17`).
- **Local DB:** `(localdb)\MSSQLLocalDB` / `Roadlab_Local`. Connection override is in `Interon.Roadlab.Web.v13/appsettings.Development.json`. Has `MultipleActiveResultSets=True` set — required by v17's `DocumentUrlAliasService`.
- **Production DB:** Azure SQL connection string lives in `appsettings.json`. Has known plaintext credentials (rotation pending — see TODO).

## Architecture

Two-project solution:
- **`Interon.Roadlab.Web.v13/`** — the Umbraco web app. (Project name is a v13 historical artifact; still on this name post-upgrade.)
- **`Interon.Roadlab.Web.Net.Core/`** — library with controllers, view models, services, generated ContentModels.

Key conventions:
- **ModelsBuilder** runs in `SourceCodeAuto` mode and writes typed model classes to `Interon.Roadlab.Web.Net.Core/Models/ContentModels/*.generated.cs`. Don't edit those — regenerate by changing the content type and rebuilding.
- **uSync** is the schema + content sync. v17 reads from `Interon.Roadlab.Web.v13/uSync/v17/`. The original `v9/` folder is kept as a v13 reference / source for the conversion script.
- **Surface controllers** in `Interon.Roadlab.Web.Net.Core/Controllers/SurfaceControllers/` handle form posts (`ContactSurfaceController`, `VacancySurfaceController`, `ModalContactSurfaceController`).
- **Razor runtime compilation** is on (`Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`) and `RazorCompileOnBuild=false` is set in the web csproj. This is **temporary** during the view-migration window — a few views (account/transaction forms, EditorTemplates) still don't compile cleanly. Re-enable build-time Razor compile once those are fixed.

## Development setup

1. **SQL Server LocalDB** must be installed (ships with Visual Studio / SSMS).
2. Create the empty local database once:
   ```
   sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE [Roadlab_Local]"
   ```
3. Run the site:
   ```
   cd Interon.Roadlab.Web.v13
   dotnet run
   ```
4. First-run: Umbraco's install wizard at `https://localhost:44375/umbraco` — enter admin user, accept the LocalDB connection.
5. After install, in the backoffice: **Settings → uSync → Import (under Settings card)** to populate schema. Then **Content card → Import (with force)** to populate content + media.

If a fresh local DB needs a v13 → v17 content data conversion (NC→BL, MediaPicker v1→v3, Grid→BG), run:
```
pwsh tools/Convert-USync-Files-V9-To-V17.ps1
```
That transforms `uSync/v17/Content/*.config` values in-place. Re-run uSync's force-import afterwards.

The runtime auto-proxies missing media files from the live staging server via `Interon.Roadlab.Web.Net.Core/Middleware/MediaLocal.cs` when `Custom:LoadMediaFromStaging=true` in `appsettings.Development.json`. So local doesn't need a `wwwroot/media/` copy.

## Build / run / package commands

```
# Build whole solution
dotnet build Interon.Roadlab.Web.sln

# Run the web project
cd Interon.Roadlab.Web.v13
dotnet run

# Restore packages
dotnet restore

# Add a package
dotnet add Interon.Roadlab.Web.v13 package PackageName
```

## Tooling scripts

- `tools/Convert-V13-To-V17-Data.ps1` — converts existing in-DB property data (NC→BL, MediaPicker v1→v3). Idempotent.
- `tools/Convert-USync-Files-V9-To-V17.ps1` — same conversions but at the **file** level (transforms `uSync/v17/Content/*.config` so uSync's editor validation doesn't reject the v9-format JSON on import). Includes Grid→BlockGrid flattening.

Production deploy will run a packaged version of these (see `TODO.md`).

## Things to know that aren't obvious from the code

- **The mobile-app feature was removed** in `0fc27992`. Account/Transaction content types, AppSettings tree, Companies, Devices, LimsElement, and 12 mobile-specific Member properties are all gone. If you see legacy references in old branches, that's why.
- **The `Html/` folder** under `wwwroot/Html/` is ~15 MB of static pre-Umbraco HTML files. Nothing in code references it but it's still served at `/Html/*`. Deletion candidate — verify no inbound links first.
- **Generated.cs files are committed** to the repo so a fresh clone builds without Umbraco running first. If you change a content type, expect `.generated.cs` diffs.
- **uSync's second-pass import logs `Cannot save a non-current version`** — known issue, doesn't block the bulk of data being written. Tracked in TODO.

## What NOT to do

- **Never push to `origin` (Azure DevOps).** It's the live-staging-deploys remote and our work isn't ready. The push URL is disabled, but don't try to re-enable it without coordination.
- **Never commit `appsettings.json` credential edits.** Use `appsettings.Development.json` (LocalDB, harmless) or `appsettings.Local.json` (gitignored) for personal overrides.
- **Don't re-enable `RazorCompileOnBuild=true`** until the deferred view fixes (`_RenderModalContactForm`, EditorTemplates Watermark, `Landing.cshtml`'s `GetFirstBlockList`) are done. The site will fail to build.

## User profile

- George — primary maintainer, prefers terse explanations and concrete diffs over high-level design talk.
- Anton — handles Azure / DB / SMTP credential rotation. Required for any production deploy.
