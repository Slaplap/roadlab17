# TODO

Living list of remaining work on the `upgrade/v17` branch. Roughly grouped by urgency / blocker status. Add items as they come up.

Legend: **[BLOCK]** = blocks production deploy · **[SAFETY]** = security / data risk · **[CHORE]** = cleanup · **[PERF]** = performance · **[FEAT]** = new functionality · **[DOC]** = documentation

---

## Before any production deploy

- [ ] **[BLOCK][SAFETY] Credential rotation** — `appsettings.json` has plaintext Azure SQL password, SMTP password, uSync AppKey. Anton rotates each, switches prod to Azure Key Vault references (or env vars), then we force-rewrite git history on `Slaplap/roadlab17` to scrub the old values.
- [x] **[BLOCK] Re-enable Razor compile-on-build** — done. EditorTemplates Watermark + `_RenderModalContactForm` + `ModalContactSurfaceController` were dead code (0 callers) and have been deleted. `Landing.cshtml`'s `GetFirstBlockList` resolves at compile time now that `@using Interon.Roadlab.Web.v17.Extensions` is wired via `_ViewImports.cshtml`. csproj no longer has `<RazorCompileOnBuild>false</...>`.
- [ ] **[BLOCK] Production data conversion** — pre-deploy run of `tools/Convert-V13-To-V17-Data.ps1` against the production database, after a v13→v17 Umbraco upgrade migration runs and before clients hit the published cache. Wrap as a one-shot console app or PowerShell `-ConnectionString` invocation that's safe to run twice.
- [ ] **[BLOCK] uSync "Cannot save a non-current version" second-pass error** — investigated; it's a transient race condition in uSync's second-pass cross-reference re-linking, not item-specific (different items trip it on different runs — "Asphalt" / "Concrete" most often, both have a parent-republish cascade triggered by their first-pass save). Workaround: re-run the import a second time and previously-failed items succeed. **DEPLOY.md must wire this in**: prod deploy script runs uSync import twice, second run is the canonical check.
- [ ] **[SAFETY] Strip `appsettings.json` of staging secrets** before the git history is forced — even with rotation done, the OLD values shouldn't ship in the repo.
- [ ] **[CHORE] Re-enable Azure DevOps push** — when ready: `git remote set-url --push origin "<PAT URL>"`. Verify the existing `azure-pipelines.yml` works against .NET 10 / Umbraco 17 build (it's currently described in `fixes.md §7.9` as broken Xamarin config — full rewrite needed).
- [x] **[CHORE] Verify the staging-only items that didn't import via uSync** — done. Diff of `uSync/v9/Content` vs `uSync/v17/Content` showed 20 missing: 10 are intentional mobile-app deletions (account/ajax/login/registration/profile/secure-area/notificaitons/our-world/industries/test), 2 are uSync sync-delta markers (`branch` rename, `branches-landing-pages` delete), 3 are duplicates/orphans (`concrete-1`, `geotechnical-road-investigations_4uahwu0d`, `home_harmyy4a`), 3 are the Industries subtree (Civil Construction + Mining and Quarrying + 1 child blog) — **accepted loss** since Industries was folded into "WHAT WE DO". The 2 real branches (`mosselbay`, `upington`) were copied from v9 → v17 and put through `Convert-USync-Files-V9-To-V17.ps1` to fix MediaPicker v1 → v3 values; user must run uSync force-import to populate the DB.

## Open issues from view migration

- [ ] **[CHORE] Faithful Grid → BlockGrid** *(deferred — George decision 2026-05-21)* — current conversion flattens everything into one RTE block per article. Visually fine, but old blog posts can't have individual images/embeds edited as separate blocks. Revisit only if editors complain about the limitation; new posts in v17 aren't affected.
- [ ] **[CHORE] AccountTransactionCreateForm + Modal forms** — Razor views from the deleted mobile-app feature that still exist but won't compile (`_RenderAccountTransactionCreateForm.cshtml` already gone; `_RenderModalContactForm.cshtml` still here, uses `dynamic` Model). Decide: delete, or rewrite for v17.
- [ ] **[CHORE] EditorTemplates** — `Views/Shared/EditorTemplates/string.cshtml` and `emailaddress.cshtml` use removed `ModelMetadata.Watermark`. Either replace with `Description` / placeholder pattern, or delete if unused.
- [x] **[CHORE] Defensive null guards** — done. Initial pass covered HomePage / AboutPage / Projects / Services / Branch / headOffice / Landing / BlogCategories / blog partials. Follow-up sweep across the full Views tree found only two remaining gaps, both fixed: Services2.cshtml's hero-image block (replaced `HasValue` + `.Id` chain with `is IPublishedContent` pattern matching) and ServiceBlock.cshtml's `alt` attribute (added `?.` before `.AltText()` — the `src` side already had it). _RenderHead/Footer/Header/TopScripts and the Branches view came back clean.
- [x] **[CHORE] Missing BlockList element-type partials** — done. Audited the 8 candidate element types: `products` is the only one rendered via the BlockList partial system (used by Aggregate, Chemicals, Field Testing, Geotechnical, Mobile Lab Services, Non-Destructive Forensic, Specialised Testing) and now has a partial that mirrors the old Services.cshtml rendering pattern. `stringItem`, `aboutTeamMembers`, and `project` are rendered directly via typed-model access in their parent templates (BlogArticle, AboutPage, Projects) and don't go through the partial system. The other four (`aboutOperationsTeams`, `branchElement`, `provinceSection`, `recentArticles`) have zero content references — defined as doctypes but never used; safe to leave without partials.

## Performance

- [x] **[PERF] CSS / JS bundling + minification** — done for CSS. Added `LigerShark.WebOptimizer.Core 3.0.477`, wired up in `Startup.cs` with one bundle `/css/site.bundle.css` combining style/animations/toast/animate/megamenu-responsive-fix (~164 KB unminified → ~114 KB minified, 5 requests → 1). JS bundling was not done — only one local JS file exists (`/js/animations.js`, 4 KB), no benefit. CDN-loaded CSS/JS (Bootstrap, Lightbox, jQuery, Google Fonts) is unchanged; consolidating those is `#4d`.
- [x] **[PERF] Lazy-load images** — done. Bulk-added `loading="lazy"` to 53 `<img>` tags across 20 .cshtml files. Skipped the hero `<img class="hero__image">` on Landing / Services / Services2 and the top-nav logo in `_RenderHeader.cshtml` — those stay eagerly loaded since they're above the fold.
- [x] **[PERF] Replace Revolution Slider** — turned out to be a delete rather than a replace. Confirmed with George that no page actually uses the slider on v17, so removed all 38 themepunch/revolution.extension JS files + `css/settings.css` (~1.6 MB) plus the dead `setREVStartSize()` inline function from `_RenderBottomScripts.cshtml`. Facebook SDK + `<div id="fb-root">` also gone (not used either). Same pass dropped the now-orphaned `onModalSubmit` JS function (modal contact form was already removed earlier).
- [ ] **[PERF] Consolidate CDN requests** *(deferred — George decision 2026-05-21)* — most CDNs that remain (GTM, reCAPTCHA, Facebook removed) must stay on their own origin for security reasons. Of Bootstrap / jQuery / FontAwesome / Lightbox2 / Popper, only 3-4 could move, and HTTP/2 connection reuse makes the gain marginal. Revisit only if Lighthouse flags this specifically.
- [x] **[PERF] Delete `wwwroot/Html/` legacy folder (~15 MB)** — done. 139 files / 15 MB of pre-Umbraco static HTML removed. Verified zero references from any .cshtml/.cs/.csproj/.js/.css/.scss/.md/.config in the solution before deletion. The csproj had one stray `<Content Include="wwwroot\Html\.vscode\launch.json" />` that was also stripped.
- [ ] **[PERF] Remove duplicate / unused assets** identified in `fixes.md §2`: `material.css` ✅ done · `pe-icon-7-stroke` fonts ✅ done · animate.css duplicate ✅ done · check unused `font-awesome/` local copy (CDN is loaded instead).
- [ ] **[PERF] Image optimization** *(deferred — George decision 2026-05-21)* — re-encoding 1,386 media files to WebP/AVIF would save ~25-50% bandwidth per image but only ~200-500 ms on broadband (lazy-loading from #4b already absorbed the bigger chunk of perceived load time). Cheapest partial win when revisited: enable `ImageSharp.Web` format negotiation via `Accept: image/webp,image/avif` headers (~1-line config in `Startup`) without re-encoding source files.

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

- [x] **[SAFETY] Add security headers middleware** — partial. Added `Net.Core/Middleware/SecurityHeadersMiddleware` setting `X-Content-Type-Options: nosniff`, `X-Frame-Options: SAMEORIGIN`, `Referrer-Policy: strict-origin-when-cross-origin` on every response. HSTS via `app.UseHsts()` (only fires outside Development, so localhost stays unpinned). **CSP intentionally deferred** — adding it without first running in report-only mode is likely to break the backoffice / Revolution Slider / Lightbox / CDN scripts; revisit as its own item.
- [x] **[SAFETY] Replace `Html.Raw(TempData["script"])` pattern** — done. Both contact and vacancy form partials no longer interpolate server-side strings into a `<script>` tag. The result `<p>` now carries `id="formResult"` and a tiny inline script (`document.getElementById('formResult')?.scrollIntoView()`) handles the post-submit scroll without any Html.Raw. Controllers no longer set `TempData["script"]`. (As a side effect, the contact form's scrollIntoView target now actually exists — the old `contactformResult` ID wasn't in the DOM, so the old code was silently no-op'ing.)
- [x] **[SAFETY] Fix silent `catch` blocks** — done. `VacancySurfaceController` injects `ILogger<>` and the spam-check failure catch now logs a warning instead of swallowing the exception silently. Legitimate applications still go through; failures are now visible in `umbraco/Logs/UmbracoTraceLog.*`.
- [x] **[SAFETY] PII in logs** — done. Added `PiiMasking.MaskEmail` helper in `Net.Core/Logging/` (preserves first char of local part + domain, e.g. `g***@interon.co.za`). Applied to all 6 user-email log sites in `ContactSurfaceController` and `SpamFilterService`. Also dropped the full email body from the SMTP error log — exception trace covers the failure context, body added raw PII. Admin recipient addresses in `Email To` / `Email From` lines left unmasked since those are config, not user-submitted.
- [ ] **[SAFETY] Error messages to users** — `TempData["Result"] = ex.Message` exposes internals; substitute generic copy and log details server-side.
- [x] **[SAFETY] Hardcoded `_remoteServerUrl`** in `MediaLocal.cs` — done. URL now read from `Custom:RemoteMediaUrl` in `appsettings.Development.json` (with the staging URL kept as a code-level fallback so behaviour is preserved if config is missing). Silent `HttpRequestException` catch now logs a `LogWarning` too.

## Backoffice content / data hygiene

- [ ] **[CHORE] Decide on `compTestingType` + `test` doctype** — `fixes.md §5.1` flagged as suspicious orphans. Confirm not used on prod, then delete.
- [ ] **[CHORE] Delete duplicate "(1)" NC and Branch doctypes** — `Branch1`, `QuoteRequest...NestedContent (1)`, `Team Member - Nested Content (1)`. Consolidate or remove. (Note: `Services2` is **not** a duplicate of `Services` — they're intentionally separate templates that may diverge; keep both.)
- [ ] **[CHORE] Rename data-types from "...Nested Content" suffix to "...Block List"** — purely cosmetic; the editor is already BlockList.
- [ ] **[CHORE] Clean up the v9 staging-export duplicate-key bugs** — staging has two content nodes sharing the same Key in a few places (Mosselbay/George, Upington/Lichtenburg, etc.). Fix on staging at source if possible.
- [ ] **[CHORE] Old grid editor partials** — `Views/Partials/grid/editors/*` already removed; verify no straggler references.

## Documentation

- [ ] **[DOC] Update `fixes.md` final state** — most sections are now historical context. Mark completed items, drop sections that no longer apply.
- [ ] **[DOC] Write a `DEPLOY.md`** — step-by-step prod deploy runbook (credential rotation → uSync schema sync → data conversion script → cache rebuild → smoke test).
- [ ] **[DOC] Inline comments on tools/** — both PS scripts have header comments but could use more inline explanation of the JSON-shape transformations.

## Maybe later / out of scope

- [ ] **[SAFETY] Content-Security-Policy** — split off from the main security-headers task. Add CSP in `Content-Security-Policy-Report-Only` mode first, watch the browser console for violations across the public site **and** Umbraco backoffice (Revolution Slider, Lightbox, Bootstrap CDN, jQuery CDN, YouTube embeds, Facebook SDK, Google reCAPTCHA, Anthropic API requests from backoffice tools are all candidates). Once the policy is tight, switch to enforcement.


- [ ] Bootstrap 5 + responsive overhaul as a separate project.
- [ ] Replace Syncfusion notification grid (already removed) with a lightweight `<table>` if any in-backoffice notifications view is still needed (currently nothing references it).
- [ ] Re-implement a customer/login area if business decides to bring back the mobile-app feature in a new shape.

---

_Add new items above or under the most relevant section. Don't worry about strict ordering — anything that ends up under "Before any production deploy" is what needs to be done first._
