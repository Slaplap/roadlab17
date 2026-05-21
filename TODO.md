# TODO

Living list of remaining work on the `upgrade/v17` branch. Roughly grouped by urgency / blocker status. Add items as they come up.

Legend: **[BLOCK]** = blocks production deploy · **[SAFETY]** = security / data risk · **[CHORE]** = cleanup · **[PERF]** = performance · **[FEAT]** = new functionality · **[DOC]** = documentation

---

## Before any production deploy

- [ ] **[BLOCK][SAFETY] Credential rotation** — `appsettings.json` has plaintext Azure SQL password, SMTP password, uSync AppKey. Anton rotates each, switches prod to Azure Key Vault references (or env vars), then we force-rewrite git history on `Slaplap/roadlab17` to scrub the old values.
- [ ] **[BLOCK] Re-enable Razor compile-on-build** — currently deferred. Fix the remaining broken views first:
    - `Views/Shared/EditorTemplates/string.cshtml` and `emailaddress.cshtml` — call removed `ModelMetadata.Watermark`
    - `Views/Partials/_RenderModalContactForm.cshtml` — dynamic-Model expression-tree errors
    - `Views/Landing.cshtml` — uses `GetFirstBlockList` extension (currently resolved via `_ViewImports.cshtml`; runtime-only, no compile check)
    - Remove `<RazorCompileOnBuild>false</...>` and `<RazorCompileOnPublish>false</...>` from `Interon.Roadlab.Web.v17.csproj` once views compile cleanly.
- [ ] **[BLOCK] Production data conversion** — pre-deploy run of `tools/Convert-V13-To-V17-Data.ps1` against the production database, after a v13→v17 Umbraco upgrade migration runs and before clients hit the published cache. Wrap as a one-shot console app or PowerShell `-ConnectionString` invocation that's safe to run twice.
- [ ] **[BLOCK] uSync "Cannot save a non-current version" second-pass error** — uSync's second-pass content save fails on a specific item. First-pass writes commit fine (visible content is intact), but cross-content references (MultiNodeTreePicker pointing to other content, etc.) may not be fully re-linked. Pin down the offending item, fix or skip it.
- [ ] **[SAFETY] Strip `appsettings.json` of staging secrets** before the git history is forced — even with rotation done, the OLD values shouldn't ship in the repo.
- [ ] **[CHORE] Re-enable Azure DevOps push** — when ready: `git remote set-url --push origin "<PAT URL>"`. Verify the existing `azure-pipelines.yml` works against .NET 10 / Umbraco 17 build (it's currently described in `fixes.md §7.9` as broken Xamarin config — full rewrite needed).
- [ ] **[CHORE] Verify the 8 staging-only items** that didn't import via uSync — investigate, add to the conversion script, or accept loss.

## Open issues from view migration

- [ ] **[CHORE] Faithful Grid → BlockGrid** — current conversion flattens everything into one RTE block per article, losing layout fidelity. Add `gridImageBlock` and `gridEmbedBlock` element types, update `Convert-USync-Files-V9-To-V17.ps1` to emit one block per Grid control rather than concatenating.
- [ ] **[CHORE] AccountTransactionCreateForm + Modal forms** — Razor views from the deleted mobile-app feature that still exist but won't compile (`_RenderAccountTransactionCreateForm.cshtml` already gone; `_RenderModalContactForm.cshtml` still here, uses `dynamic` Model). Decide: delete, or rewrite for v17.
- [ ] **[CHORE] EditorTemplates** — `Views/Shared/EditorTemplates/string.cshtml` and `emailaddress.cshtml` use removed `ModelMetadata.Watermark`. Either replace with `Description` / placeholder pattern, or delete if unused.
- [ ] **[CHORE] Defensive null guards** — done for HomePage / AboutPage / Projects / Services / Branch / headOffice / Landing / BlogCategories / blog partials. Audit other views (`branches.cshtml`, `Footer` partial, `_RenderTopScripts`, etc.) for similar `Model.Value<IPublishedContent>(x).Url()` patterns.

## Performance

- [ ] **[PERF] CSS / JS bundling + minification** — set up `Microsoft.AspNetCore.WebOptimizer` or `<bundleconfig.json>`. Combine the 7+ separate stylesheet requests; minify custom JS. ~1-2 hr.
- [ ] **[PERF] Lazy-load images** — add `loading="lazy"` to non-hero `<img>` tags in partials. ~1 hr.
- [ ] **[PERF] Replace Revolution Slider** — old + heavy (~150 KB). Use Swiper.js or CSS-only carousel. Half-day, touches hero sections.
- [ ] **[PERF] Consolidate CDN requests** — Bootstrap, jQuery, FontAwesome, Lightbox2, Popper all loaded from separate CDNs. Either bundle locally or pick a single CDN. ~30 min.
- [ ] **[PERF] Delete `wwwroot/Html/` legacy folder (~15 MB)** — pre-Umbraco static HTML files, nothing in code references them. Verify no inbound bookmarks first.
- [ ] **[PERF] Remove duplicate / unused assets** identified in `fixes.md §2`: `material.css` ✅ done · `pe-icon-7-stroke` fonts ✅ done · animate.css duplicate ✅ done · check unused `font-awesome/` local copy (CDN is loaded instead).
- [ ] **[PERF] Image optimization** — many of the 1,386 imported media files are likely larger than they need to be. Run them through a re-encoder (avif/webp where supported).

## Bootstrap / frontend libraries

- [ ] **[CHORE] Bootstrap 4.3.1 → 5.x** — covered in `fixes.md §3`. Class renames (`ml-*` → `ms-*` etc.), `data-bs-*` attributes, mega-menu rebuild. Half-day to full-day.
- [ ] **[CHORE] Replace `node-sass` with `sass` (dart-sass)** — `node-sass 4.12.0` is deprecated.
- [ ] **[CHORE] Update or remove `Animate.css 3.7.2`** — class names changed in v4.
- [ ] **[CHORE] Replace or upgrade `Lightbox2 2.8.2`** — old.
- [ ] **[CHORE] Update jQuery 3.3.1 slim → 3.7.x** — or remove altogether once Bootstrap 5 is in (no longer required).
- [ ] **[CHORE] Inline `<style>` in views** — `_RenderModalContactForm.cshtml` and others have inline CSS. Move to stylesheets.

## SEO / accessibility

- [ ] **[FEAT] Canonical tags** — add `<link rel="canonical">` to `_RenderHead.cshtml`.
- [ ] **[FEAT] Structured data (JSON-LD)** — at minimum `LocalBusiness` on head office page, `BreadcrumbList` on inner pages.
- [ ] **[FEAT] Breadcrumb markup** — visual + schema.
- [ ] **[FEAT] Open Graph fallback** — `_RenderHead.cshtml` lines 9-11 produce a malformed URL when `oGImage` is null. Add a fallback default OG image.
- [ ] **[FEAT] Alt text quality pass** — many `alt="Header Image"` / `alt="Project"` placeholders.
- [ ] **[FEAT] Heading hierarchy** — some pages start at H2 instead of H1; audit.
- [ ] **[FEAT] iframe titles** — YouTube embeds in AboutPage / BlogArticle should all have `title=`.

## Security (post-credential-rotation)

- [ ] **[SAFETY] Add security headers middleware** — CSP, X-Frame-Options, X-Content-Type-Options, HSTS. v17 makes `UseHttps=true` default; add the rest.
- [ ] **[SAFETY] Replace `Html.Raw(TempData["script"])` pattern** — `fixes.md §1.5`. Use a safer post-submit hook (data-attr-driven JS instead of injected `<script>`).
- [ ] **[SAFETY] Fix silent `catch` blocks** — `VacancySurfaceController.cs` line ~70 (now post-cleanup) still has empty catch with TODO comment. Wire up `ILogger` and at minimum `Log.Warning(ex)`.
- [ ] **[SAFETY] PII in logs** — `ContactSurfaceController.cs` logs email addresses. Mask before logging.
- [ ] **[SAFETY] Error messages to users** — `TempData["Result"] = ex.Message` exposes internals; substitute generic copy and log details server-side.
- [ ] **[SAFETY] Hardcoded `_remoteServerUrl`** in `MediaLocal.cs` — move to config.

## Backoffice content / data hygiene

- [ ] **[CHORE] Decide on `compTestingType` + `test` doctype** — `fixes.md §5.1` flagged as suspicious orphans. Confirm not used on prod, then delete.
- [ ] **[CHORE] Delete duplicate "(1)" NC and Branch doctypes** — `Branch1`, `Services2` (vs `Services`), `QuoteRequest...NestedContent (1)`, `Team Member - Nested Content (1)`. Consolidate or remove.
- [ ] **[CHORE] Rename data-types from "...Nested Content" suffix to "...Block List"** — purely cosmetic; the editor is already BlockList.
- [ ] **[CHORE] Clean up the v9 staging-export duplicate-key bugs** — staging has two content nodes sharing the same Key in a few places (Mosselbay/George, Upington/Lichtenburg, etc.). Fix on staging at source if possible.
- [ ] **[CHORE] Old grid editor partials** — `Views/Partials/grid/editors/*` already removed; verify no straggler references.

## Documentation

- [ ] **[DOC] Update `fixes.md` final state** — most sections are now historical context. Mark completed items, drop sections that no longer apply.
- [ ] **[DOC] Write a `DEPLOY.md`** — step-by-step prod deploy runbook (credential rotation → uSync schema sync → data conversion script → cache rebuild → smoke test).
- [ ] **[DOC] Inline comments on tools/** — both PS scripts have header comments but could use more inline explanation of the JSON-shape transformations.

## Maybe later / out of scope

- [ ] Bootstrap 5 + responsive overhaul as a separate project.
- [ ] Replace Syncfusion notification grid (already removed) with a lightweight `<table>` if any in-backoffice notifications view is still needed (currently nothing references it).
- [ ] Re-implement a customer/login area if business decides to bring back the mobile-app feature in a new shape.
- [ ] Convert `wwwroot/Html/` static pages to Umbraco doctypes if any of them are still needed.

---

_Add new items above or under the most relevant section. Don't worry about strict ordering — anything that ends up under "Before any production deploy" is what needs to be done first._
