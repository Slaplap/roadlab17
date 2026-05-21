# Roadlab.co.za — Umbraco CMS

Corporate website for Roadlab (materials testing, civil-engineering services).

- **Production:** [roadlab.co.za](https://roadlab.co.za/) (live)
- **Staging:** [roadlabstaging.azurewebsites.net](https://roadlabstaging.azurewebsites.net/) — Umbraco 13, current source of truth
- **Upgrade branch:** `upgrade/v17` — Umbraco 17.4.1 / .NET 10 migration, not deployed yet

## Stack

| | |
|---|---|
| CMS | Umbraco 17.4.1 (was 13.2.2) |
| .NET | net10.0 (was net8.0) |
| DB | Azure SQL (prod/staging) · SQL Server LocalDB (dev) |
| Schema sync | uSync 17.3.2 + uSync.Complete 17.3.6 |
| Search | Our.Umbraco.FullTextSearch 17.0.1 |
| Hosting | Azure App Service |

## Solution layout

```
Interon.Roadlab.Web.sln
├── Interon.Roadlab.Web.v13/          ← web app (project name is v13-era, still on it)
│   ├── Views/                          Razor templates
│   ├── wwwroot/                        static CSS/JS/images/media
│   ├── uSync/v17/                      schema + content sync (Umbraco 17 layout)
│   ├── uSync/v9/                       legacy v13 export, source for conversion script
│   ├── umbraco/                        Umbraco runtime data (gitignored except sub-keepers)
│   ├── Startup.cs / Program.cs
│   └── appsettings*.json
├── Interon.Roadlab.Web.Net.Core/     ← class library
│   ├── Controllers/                    surface + MVC controllers
│   ├── Models/ContentModels/           ModelsBuilder-generated content classes
│   ├── Models/ViewModels/              hand-written view models for forms
│   ├── Services/                       SpamFilterService etc.
│   ├── Config/                         EmailSettings, etc.
│   └── Middleware/MediaLocal.cs        proxies missing media from staging in dev
├── tools/                              one-off migration scripts (PowerShell)
├── CLAUDE.md                           guide for AI assistants working here
├── TODO.md                             prioritized punch-list of remaining work
├── fixes.md                            historical audit + per-cluster migration notes
└── README.md                           you are here
```

## Local development

You need:
- .NET 10 SDK
- SQL Server LocalDB (bundled with Visual Studio / SSMS Express)

One-time setup:
```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE [Roadlab_Local]"
cd Interon.Roadlab.Web.v13
dotnet run
```

The local connection string is hard-coded in `appsettings.Development.json` (LocalDB, integrated auth) and overrides the production string in `appsettings.json`. ASP.NET Core's environment-aware config will use the Development override automatically when `ASPNETCORE_ENVIRONMENT=Development` (the default for `dotnet run`).

On first boot Umbraco shows its install wizard at `https://localhost:44375/umbraco`. After install, populate schema + content via the uSync dashboard (Settings → uSync). See `CLAUDE.md` for the full flow.

Missing media files are auto-proxied from `roadlabstaging.azurewebsites.net` in development — you don't need a local media copy.

## Deploy notes (not yet wired up)

The current Azure App Service deploys from the Azure DevOps `origin` remote on push to `master`. The push URL is intentionally disabled on this branch so unfinished v17 work can't go live. Reactivate with:
```bash
git remote set-url --push origin "<azure-devops-PAT-URL>"
```

Before any v17 deploy:
1. Rotate the DB / SMTP / uSync credentials currently in `appsettings.json` (Anton).
2. Run `tools/Convert-V13-To-V17-Data.ps1` against the production database after the schema sync.
3. Verify Razor views compile (re-enable `RazorCompileOnBuild=true` after fixing the deferred view errors).
4. See `TODO.md` for the full pre-deploy checklist.

## Branches

| Branch | Purpose | Remote |
|---|---|---|
| `master` | live staging deploy source (v13) | `origin` (Azure DevOps) |
| `staging` | live staging working branch (v13) | `github` |
| `upgrade/v17` | the v17 / .NET 10 migration (this work) | `github` |

## Reference

- `CLAUDE.md` — context for AI-assisted edits, dev setup, things-not-obvious-from-code
- `TODO.md` — what's left
- `fixes.md` — full audit + per-feature migration notes, kept for history
