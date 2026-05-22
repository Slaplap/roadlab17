# Roadlab Site - Comprehensive Analysis & Recommendations

**Date written:** 2026-04-13
**Original stack:** Umbraco 13.2.2 / .NET 8 / Bootstrap 4.3.1
**Target stack:** Umbraco 17.4.1 / .NET 10 / Bootstrap 5.x (BS5 upgrade is deferred — see TODO.md #6)

---

> **Status note (2026-05-22):** This document is historical. The vast majority
> of the items inventoried here have been addressed during the `upgrade/v17`
> migration. For the live punch-list — what's still open, what's deferred,
> what's done — read **`TODO.md`**, not this file. Treat the sections below
> as background context (what the codebase looked like before the migration)
> rather than a current to-do list.
>
> Items in this file that are still meaningfully open are mirrored into
> `TODO.md` with current status; everything else (security fixes, CSS dedup,
> view null-guards, deprecated property editors, SEO, performance, etc.)
> has already been worked through in this branch's commit history.

---

## TABLE OF CONTENTS

1. [Critical Security Issues](#1-critical-security-issues)
2. [CSS & Frontend Issues](#2-css--frontend-issues)
3. [Bootstrap Upgrade (4.3.1 to 5.x)](#3-bootstrap-upgrade-431-to-5x)
4. [Document Types & Content Model Issues](#4-document-types--content-model-issues)
5. [Deprecated Property Editors](#5-deprecated-property-editors)
6. [Razor Views & Templates](#6-razor-views--templates)
7. [Controller & Backend Code Issues](#7-controller--backend-code-issues)
8. [JavaScript Issues](#8-javascript-issues)
9. [SEO & Accessibility](#9-seo--accessibility)
10. [Performance & Build Pipeline](#10-performance--build-pipeline)
11. [Umbraco 13 to 17 Upgrade Plan](#11-umbraco-13-to-17-upgrade-plan)

---

## 1. CRITICAL SECURITY ISSUES

### 1.1 Secrets Hardcoded in appsettings.json — PENDING (needs Anton)
- **File:** `Interon.Roadlab.Web.v17/appsettings.json` (lines 34, 44, 54)
- Database password, SMTP password, and uSync AppKey are committed to source control. The repo is pushed to a private GitHub (`Slaplap/roadlab17`), so exposure is limited to that account, but rotation is still required before any public/wider sharing.
- **Coordinated fix needed:**
  1. Anton rotates the Azure SQL `interon` user password in Azure Portal.
  2. Anton rotates the SMTP `webserver@roadlab.co.za` password on the mail server.
  3. Anton generates a new uSync Publisher AppKey.
  4. Production reads all three from Azure Key Vault references (or environment variables). Repo's `appsettings.json` keeps placeholder strings only.
  5. After cutover, force-rewrite history on `Slaplap/roadlab17` to scrub the old values (git filter-branch / BFG).
- Cannot be done unilaterally — needs coordination so the live staging site doesn't break mid-rotation.

### 1.2 File Upload Path Traversal Vulnerability — FIXED (2026-05-20, branch `upgrade/v17`)
- **File:** `VacancySurfaceController.cs`
- Added 5MB size cap, MIME whitelist (PDF/Word only) + extension whitelist (`.pdf`, `.doc`, `.docx`), GUID-based filenames, saves to `App_Data/VacancyUploads/` (outside wwwroot, so files aren't web-accessible), guarded email-attachment call against the null-file case.
- **Still TODO (separate cleanup):** the empty `catch` in the spam-check block (lines ~70-73) silently swallows exceptions. Comment says "Log but don't block" but no actual `ILogger` call. Needs a logger injection.

### 1.3 Debugger Statements in Production Code — FIXED (2026-05-20, branch `upgrade/v17`)
- Removed from `Views/Partials/_RenderBottomScripts.cshtml` (3 instances in reCAPTCHA callbacks) and `wwwroot/js/roadlab.js` (1 instance in openDetails click handler).

### 1.4 External Third-Party Script
- **File:** `_RenderHead.cshtml` (line 62)
- Loading `https://refactorengine-production.up.railway.app/plugin.js` with API communication to `ai-auditor.interon.co.za`
- **Review:** Verify this is intentional and trusted. External scripts are a supply-chain risk.

### 1.5 Html.Raw() on TempData
- Contact forms render `TempData["script"]` with `@Html.Raw()` - potential XSS if script content is tainted
- **Fix:** Use a safer pattern for post-submit script injection

---

## 2. CSS & FRONTEND ISSUES

### 2.1 Massive Unused CSS: material.css (3.9 MB)
- Syncfusion EJ2 Material theme - accounts for ~94% of CSS payload
- Only used for a legacy EJ2 Grid in `roadlab.js` (line 240)
- **Fix:** Remove Syncfusion dependency entirely, or replace with a lightweight data table. This single file is larger than most entire websites.

### 2.2 Outdated Libraries
| Library | Current | Latest | Action |
|---------|---------|--------|--------|
| Bootstrap | 4.3.1 (2019) | 5.3.x | Upgrade (see Section 3) |
| jQuery | 3.3.1 slim | 3.7.x | Upgrade or remove (Bootstrap 5 doesn't need it) |
| node-sass | 4.12.0 | Deprecated | Replace with `sass` (dart-sass) |
| Animate.css | 3.7.2 | 4.1.x | Upgrade (class name changes) |
| Lightbox2 | 2.8.2 | 2.11.x | Upgrade or replace with modern alternative |
| Revolution Slider | 5.4.5 | 6.x | Consider replacing with lightweight alternative (Swiper.js) |

### 2.3 Excessive !important Usage (356 instances)
- `megamenu-responsive-fix.css` alone has 227 `!important` declarations
- Indicates CSS specificity wars from layered overrides
- **Fix:** Refactor mega menu CSS to use proper specificity instead of brute-force overrides

### 2.4 Duplicate CSS Files
- `animate.css` exists in both `/css/` and `/Html/css/` (80 KB duplicated)
- `animations.css` has different versions in both locations
- **Fix:** Remove `/Html/css/` duplicates, keep only `/css/` as the served directory

### 2.5 Unused Font Files
- `pe-icon-7-stroke/` font files in `/wwwroot/fonts/` - commented out in HTML
- Local `font-awesome/` files unused (CDN version loaded instead)
- **Fix:** Remove unused font directories

### 2.6 No CSS Bundling or Minification
- 7+ CSS files loaded individually with no concatenation or minification
- No `bundleconfig.json`, webpack, or gulp setup
- **Fix:** Implement ASP.NET bundling (`WebOptimizer`) or build-time bundling. Minify all custom CSS for production.

### 2.7 Inline Styles in Views (30+ instances)
- Hardcoded `style=""` attributes throughout views for layout, spacing, display
- Examples: `display: inline-block;`, `text-align: center; padding: 50px;`, `margin-right: 10px;`
- **Fix:** Create utility CSS classes or use Bootstrap utilities

### 2.8 SCSS Compiler Outdated
- Using `node-sass 4.12.0` which is deprecated and unsupported
- **Fix:** Replace with `sass` (dart-sass) in `package.json`:
  ```json
  "devDependencies": { "sass": "^1.77.0" },
  "scripts": { "compile:sass": "sass sass/main.scss css/style.css" }
  ```

### 2.9 Inconsistent Responsive Breakpoints
- Mostly uses 768px with limited coverage at other breakpoints
- Some components use non-standard breakpoints (489px)
- **Fix:** Align with Bootstrap's breakpoint system consistently

---

## 3. BOOTSTRAP UPGRADE (4.3.1 to 5.x)

### Breaking Changes to Address

**3.1 jQuery Dependency Removed**
- Bootstrap 5 does not require jQuery
- Your site uses jQuery for: modals, AJAX, toast, Revolution Slider, form handling
- **Decision:** Keep jQuery for now (Revolution Slider needs it), but stop writing new jQuery code

**3.2 Class Name Changes**
| Bootstrap 4 | Bootstrap 5 | Files Affected |
|-------------|-------------|----------------|
| `.ml-*` / `.mr-*` | `.ms-*` / `.me-*` | All views using margin left/right |
| `.pl-*` / `.pr-*` | `.ps-*` / `.pe-*` | All views using padding left/right |
| `.float-left` / `.float-right` | `.float-start` / `.float-end` | Various views |
| `.text-left` / `.text-right` | `.text-start` / `.text-end` | Various views |
| `.badge-*` | `.bg-*` on badges | If badges used |
| `.close` | `.btn-close` | Modal close buttons |
| `.sr-only` | `.visually-hidden` | _RenderHeader.cshtml |
| `.form-group` | Removed (use spacing) | All forms |
| `.form-row` | `.row` | Forms |
| `.custom-control` | `.form-check` | Checkboxes/radios |
| `.input-group-append/prepend` | Removed (direct children) | Form inputs |
| `data-toggle` | `data-bs-toggle` | Navbar, modals, dropdowns |
| `data-target` | `data-bs-target` | Navbar, modals |
| `data-dismiss` | `data-bs-dismiss` | Modal close buttons |
| `data-parent` | `data-bs-parent` | Accordion |

**3.3 JavaScript API Changes**
```javascript
// Bootstrap 4 (jQuery)
$('#myModal').modal('show');

// Bootstrap 5 (vanilla JS)
const modal = new bootstrap.Modal(document.getElementById('myModal'));
modal.show();
```

**3.4 Grid System Changes**
- `col-xs-*` officially gone (was already deprecated in BS4, use `col-*`)
- New `xxl` breakpoint added (1400px)
- RTL support added via `.ms-*`/`.me-*`

**3.5 Mega Menu Impact**
- The mega menu heavily uses Bootstrap 4 navbar patterns
- The 227 `!important` overrides in `megamenu-responsive-fix.css` will likely need complete rewrite
- **Recommendation:** Rebuild mega menu from scratch using Bootstrap 5 patterns

---

## 4. DOCUMENT TYPES & CONTENT MODEL ISSUES

### 4.1 Duplicate/Redundant Branch Types
- `Branch` and `Branch1` ("Branch listing") have nearly identical properties
- Both have address, contact, location fields
- Branch1 lacks some properties (Image, ImageAlt, BackgroundImage, limsBranchCode)
- **Fix:** Consolidate into a single Branch document type with optional properties

### 4.2 Conflicting Service Document Types
- `Services` - Full service page with hero/heading/SEO compositions
- `Services2` - Alternative with pageBlockContent instead of pageHeading
- `ServiceNew` - Element type in Account folder, no template
- **Fix:** Clarify purpose. If Services2 replaces Services, remove the old one. If different purposes, rename clearly.

### 4.3 Orphaned Document Types
- `releatedArticles` (typo) marked for DELETION in uSync
- `relatedArticles` marked as RENAMED in uSync
- Cleanup was attempted but not completed
- **Fix:** Complete the cleanup, remove orphaned uSync files

### 4.4 Inconsistent Naming Conventions
- `sEO` property alias (should be `seo` or `SEO`)
- `_Blog Selector` composition name (spaces, should be `_BlogSelector`)
- Mix of camelCase and PascalCase across document types
- **Fix:** Standardize naming. Use camelCase for aliases, PascalCase for names.

### 4.5 ServicesList Has No Template
- Defined as a content type but has no DefaultTemplate assigned
- Acts as container for ServicesType children
- **Fix:** Add a listing template or restructure

### 4.6 Over-Engineered Account Structure
- 12 Account-related document types
- Some (AccountPasswordChange, AccountPasswordRequest) are very simple single-purpose pages
- **Consider:** Can some be consolidated or simplified?

---

## 5. DEPRECATED PROPERTY EDITORS

**These MUST be migrated before upgrading to Umbraco 14+. They are removed entirely.**

### 5.1 Umbraco.NestedContent (14 usages) --> Block List
- **STATUS (2026-05-20):** 13 of 14 schema-migrated to `Umbraco.BlockList` on branch `upgrade/v17`. uSync configs and Razor views (`BlogArticle.cshtml`, `AboutPage.cshtml`, `Projects.cshtml`, `Services.cshtml`) updated for the new iteration pattern (`(ElementType)block.Content`).
- **`AppSettingsTestingTypesNestedContent` — DELETED (2026-05-20, branch `upgrade/v17`).** Confirmed orphaned: no content type references its key `c8284069-...`, no Razor/C# code uses `testingTypes` or `AppSettingsTestingTypes`, and its `ncAlias` pointed at a non-existent element type. Replaced the file with a uSync `<Empty Change="Delete">` marker so production drops it on the next uSync import too.
- **`compTestingType` element + `test` content type — flagged for future cleanup, NOT deleted.** Both look orphaned in this codebase (the `test` content type has no template, no properties of its own, no children allowed — just a composition wrapping `compTestingType` which itself only has `testingTypeName` + `testingTypeDescription` textboxes). Left in place because production may have content nodes of type `test` that we can't safely delete without confirmation from Anton. **Action:** verify on prod, then either keep or delete via `<Empty Change="Delete">` markers like above.
- **Duplicate/orphan NC data types still to clean up:**
  - `AboutPageOperationTeamsNestedContent1` (`429a457d-...`) duplicates `OperationTeamsNestedContent` — both reference `aboutOperationsTeams`.
  - `QuoteRequestQuoteRequestLineItemsNestedContent1` (`549b4804-...`) duplicates `QuoteRequestQuoteRequestLineItemsNestedContent`.
  - `TeamMemberNestedContent1` (`d5f2a503-...`) duplicates `TeamMemberNestedContent`.
  - Migrated to BL for consistency, but no content type references them — safe to delete in cleanup pass.
- **Production data migration (still TODO):** uSync only changes schema, not existing content data. The old NC JSON in `umbracoPropertyData.dataNvarchar` is not auto-converted to BL JSON. Before deploying this to prod, write a one-off SQL/code migration script that walks affected property records and rewrites the JSON from NC format `[{key,name,ncContentTypeAlias,...}]` to BL format `{layout:{...},contentData:[{contentTypeKey,udi,...}],settingsData:[]}`.

### 5.2 Umbraco.Grid (1 usage: Blog Article) --> Block Grid — SCHEMA DONE (2026-05-20, branch `upgrade/v17`)
- `BlogArticleGrid` data type converted from `Umbraco.Grid` to `Umbraco.BlockGrid`. Created new element type `blogContentBlock` (key `9a4f7c2e-1b3d-4e8a-bf01-7d62a8f5c930`) with a single `content` RTE property as the initial block.
- `blogarticle.config`: property `grid` Type updated from `Umbraco.Grid` → `Umbraco.BlockGrid`. ModelsBuilder regenerated `BlogArticle.Grid` from `JToken` → `BlockGridModel`.
- `BlogArticle.cshtml`: `@Html.GetGridHtml(Model, "grid")` replaced with `@await Html.GetBlockGridHtmlAsync(Model.Grid)`.
- New partial: `Views/Partials/blockgrid/Components/blogContentBlock.cshtml` renders the RTE content inside `<div class="blog-content-block">`.
- **Single-block setup, expand later:** Only one block type (`blogContentBlock`) is defined. The old Grid had separate editors for media/embed/textstring — those are covered by the RTE's built-in image and embed support for now. If team wants explicit image-only or embed-only blocks, add element types and re-import via uSync.
- **Old grid editor partials still on disk:** `Views/Partials/grid/editors/{base,embed,macro,media,rte,textstring}.cshtml` are now dead code. Safe to delete in a cleanup commit (they're listed under `<None Include=>` in the .csproj so they don't compile anyway).
- **Production content migration (TODO):** Existing BlogArticles have grid JSON in the old Grid Layout shape. No auto-converter — each blog article needs manual rebuild in Block Grid editor at deploy time. (Alternative: write a one-off script that extracts text/HTML content from the legacy grid JSON and creates BlockGrid items.)

### 5.3 Umbraco.MediaPicker (12 usages) --> MediaPicker3 — SCHEMA DONE (2026-05-20, branch `upgrade/v17`)
- All 12 v1 data types converted to `Umbraco.MediaPicker3` editor with the equivalent v3 config (`OnlyImages: true` → `Filter: "umbracoMediaImage"`, `DisableFolderSelect` dropped, added `Crops/EnableLocalFocalPoint/ValidationLimit` defaults).
- Build clean with no view changes needed — `.Url()` is an extension method that works on both `IPublishedContent` and `MediaWithCrops`, so existing view code stayed working.
- **Production data conversion still TODO:** v1 stored single value as `umb://media/<udi>` string; v3 stores JSON array of `{key, mediaKey, crops, focalPoint}` objects. Same orphan-on-existing-data risk as the NC→BL migration. Production deploy needs a one-off conversion script.

### 5.4 Summary of Required Migrations
| Editor | Count | Replacement | Priority |
|--------|-------|-------------|----------|
| Nested Content | 14 | Block List | CRITICAL (removed in v14) |
| Grid Layout | 1 | Block Grid | CRITICAL (removed in v14) |
| Media Picker (v1) | 12 | Media Picker 3 | HIGH (deprecated) |

---

## 6. RAZOR VIEWS & TEMPLATES

### 6.1 Outdated DOCTYPE
- **File:** `MasterPage.cshtml` (line 5)
- Uses XHTML 1.0 Strict DOCTYPE
- **Fix:** Change to `<!DOCTYPE html>` (HTML5)

### 6.2 Deprecated Razor Helpers
| Deprecated | Replacement | Locations |
|-----------|-------------|-----------|
| `Html.Partial()` | `@await Html.PartialAsync()` | 21+ files |
| `Html.RenderPartial()` | `@await Html.RenderPartialAsync()` | Multiple files |
| `Html.HiddenFor()` | `<input asp-for="..." type="hidden">` | _RenderModalContactForm |
| `Html.TextBoxFor()` | `<input asp-for="...">` | _RenderModalContactForm |
| `Html.ValidationMessageFor()` | `<span asp-validation-for="...">` | _RenderModalContactForm |
| `Html.ActionLink()` | `<a asp-controller="..." asp-action="...">` | _RenderHeader |

### 6.3 Hardcoded Content (Should Be CMS-Managed)
- Phone number: "+27 11 828 0279" in `_RenderHeader.cshtml` (line 8)
- Email: "info@roadlab.africa" in `_RenderHeader.cshtml` (line 11)
- Address: "207 Rietfontein Rd, Primrose, Johannesburg" in footer
- Social media URLs hardcoded in header and footer
- All branch links and mega menu service links hardcoded
- **Fix:** Create a Site Settings document type or use AppSettings to store global content

### 6.4 Typo in Property Name
- **File:** `_RenderModalContactForm.cshtml` (line 40)
- `ConactNumber` should be `ContactNumber` (missing 't')

### 6.5 Inline `<style>` Tags in Views
- **File:** `_RenderModalContactForm.cshtml` (lines 21-25)
- Inline CSS for validation error styling
- **Fix:** Move to stylesheet

### 6.6 Legacy _Layout.cshtml
- `Views/Shared/_Layout.cshtml` exists but appears unused (MasterPage.cshtml is the active layout)
- Contains Bootstrap 3 references
- **Fix:** Remove if confirmed unused

### 6.7 Magic Strings for Property Access
- Using `Model.Value("pageDescription")` with string literals throughout
- No compile-time checking for property names
- **Fix:** Use strongly-typed models: `Model.PageDescription` (ModelsBuilder provides these)

---

## 7. CONTROLLER & BACKEND CODE ISSUES

### 7.1 Duplicate Email Logic Across 3 Controllers
- `ContactSurfaceController`, `ModalContactSurfaceController`, `VacancySurfaceController`
- All contain ~150 lines of identical SMTP email sending code
- **Fix:** Extract into `IEmailService` / `EmailSender` service, inject via DI

### 7.2 Deprecated Configuration API
- **File:** `ModalContactSurfaceController.cs` (line 109)
- Uses `System.Configuration.ConfigurationManager.AppSettings["mailfrom"]`
- Rest of codebase uses `IOptions<EmailSettings>` pattern
- **Fix:** Use `IOptions<EmailSettings>` consistently

### 7.3 Silent Exception Handling
- `VacancySurfaceController.cs` (lines 70-73): Empty catch block
- `SearchController.cs` (lines 68-71): Silent catch
- `BlogSearchResults.cshtml` (lines 54-71): Try-catch in view
- **Fix:** Log all exceptions via Serilog, show generic user messages

### 7.4 Direct HttpContext/Request.Form Access
- `SearchController.cs`: `HttpContext.Request.Query["q"]` instead of `[FromQuery]`
- All email controllers: `Request.Form["message"]` instead of model binding
- **Fix:** Use `[FromQuery]` and include message in view models

### 7.5 Error Messages Exposed to Users
- `TempData["Result"] = ex.Message;` returns internal error details
- **Fix:** Return generic "something went wrong" messages; log details server-side

### 7.6 Sensitive Data in Logs
- `ContactSurfaceController.cs` (lines 116-117): Logs email addresses
- **Fix:** Mask PII in log output

### 7.7 Missing Security Headers
- No Content-Security-Policy headers
- No X-Frame-Options or X-Content-Type-Options
- No HTTPS enforcement/HSTS
- **Fix:** Add security headers middleware

### 7.8 Hard-Coded Environment URL
- **File:** `MediaLocal.cs` (line 25)
- `_remoteServerUrl = "https://roadlabstaging.azurewebsites.net/media/"`
- **Fix:** Move to appsettings.json configuration

### 7.9 Broken Azure Pipeline
- **File:** `azure-pipelines.yml`
- Triggers on "masterAndroid" branch - appears to be old Xamarin config
- Contains Xamarin Android build tasks that don't match Umbraco project
- **Fix:** Rewrite pipeline for Umbraco .NET deployment

---

## 8. JAVASCRIPT ISSUES

### 8.1 Global Variables
- `roadlab.js` uses global `var appSettings`, `var addIndex`, `var idx`, `var testArray`
- **Fix:** Use module pattern, IIFE, or ES modules

### 8.2 console.log in Production
- **File:** `animations.js` (line 130)
- **Fix:** Remove all console.log statements

### 8.3 Inline onclick Handlers
- **File:** `AllBranchesPage.cshtml` (lines 86, 92)
- `onclick="addToPhone(...)"` and `onclick="openMapWithCoords(...)"`
- **Fix:** Use event delegation with `addEventListener`

### 8.4 Multiple jQuery Versions
- jQuery 3.3.1 slim loaded from CDN in `_RenderTopScripts`
- jQuery 3.4.1 referenced in legacy `_Layout.cshtml`
- **Fix:** Consolidate to single version

---

## 9. SEO & ACCESSIBILITY

### 9.1 Missing SEO Elements
- No canonical tags (`<link rel="canonical">`)
- No structured data (Schema.org / JSON-LD)
- No hreflang tags for international content
- No breadcrumb markup
- **Fix:** Add canonical tags, implement LocalBusiness and BreadcrumbList schema

### 9.2 Accessibility Gaps
- Generic alt text: `alt="Header Image"`, `alt="Project"` (too vague)
- YouTube iframes missing `title` attributes (inconsistent)
- Some pages start with H2 instead of H1 (heading hierarchy)
- **Fix:** Descriptive alt text, consistent heading hierarchy, iframe titles

### 9.3 Open Graph Image Issue
- **File:** `_RenderHead.cshtml` (lines 9-11)
- If `oGImage` property is null, `ogImage` URL becomes malformed
- **Fix:** Add null check with fallback default OG image

---

## 10. PERFORMANCE & BUILD PIPELINE

### 10.1 CSS Payload: ~4.2 MB Total
| File | Size | Action |
|------|------|--------|
| material.css | 3.9 MB | REMOVE (replace Syncfusion grid) |
| animate.css | 80 KB | Upgrade to v4 or replace with CSS-only |
| style.css | 41 KB | Minify |
| animations.css | 21 KB | Merge into style.css |
| settings.css | 30 KB | Remove if Revolution Slider replaced |
| megamenu-responsive-fix.css | 6.2 KB | Rebuild for BS5 |
| toast.css | 5.4 KB | Minify and merge |
| blog.css | 551 B | Merge into style.css |

**Potential savings: ~4 MB (95% reduction) by removing material.css alone**

### 10.2 No Production Build Optimisation
- No minification of custom CSS/JS
- No bundling of multiple files
- No CSS purging/tree-shaking
- **Fix:** Add WebOptimizer or a build-time bundling step

### 10.3 Multiple CDN Requests
- Bootstrap CSS from stackpath CDN
- Bootstrap JS from stackpath CDN
- Popper.js from cdnjs
- Font Awesome from kit.fontawesome.com
- Lightbox2 from cdnjs
- Google Fonts API
- reCAPTCHA API
- **Consider:** Self-hosting critical assets to reduce DNS lookups and improve reliability

---

## 11. UMBRACO 13 TO 17 UPGRADE PLAN

### What Changed Between Versions

| Version | .NET | Key Changes |
|---------|------|-------------|
| **14** | .NET 8 | New Bellissima backoffice (AngularJS removed), Nested Content & Grid removed, Macros removed, Newtonsoft.Json replaced with System.Text.Json |
| **15** | .NET 9 | `IPublishedSnapshot` removed, ModelsBuilder models must regenerate |
| **16** | .NET 9 | TinyMCE removed (replaced by TipTap RTE), `Umbraco.Cms.Web.Backoffice` package removed |
| **17** | .NET 10 | UTC date storage, NPoco 6.x, Razor compile settings removed, UseHttps default true |

### Recommended Upgrade Strategy

You can go directly from 13 to 17 (no need to step through each version), but preparation is essential.

---

### PHASE 0: Pre-Upgrade Preparation (Do on v13)

**This is the most important phase. Do all of this before touching the Umbraco version.**

- [ ] **Migrate Nested Content to Block List** (14 usages across document types)
- [ ] **Migrate Grid Layout to Block Grid** (1 usage on BlogArticle)
- [ ] **Migrate legacy Media Picker to MediaPicker3** (12 usages)
- [ ] **Remove Macros** if any exist (check `Views/MacroPartials/`)
- [ ] **Fix all security issues** from Section 1
- [ ] **Remove debugger statements** and console.logs
- [ ] **Remove unused CSS/JS** (material.css, unused fonts)
- [ ] **Take full database backup and uSync export**
- [ ] **Document all custom backoffice extensions** (if any)

### PHASE 1: .NET and Package Updates

- [ ] Update `TargetFramework` from `net8.0` to `net10.0` in both `.csproj` files
- [ ] Install .NET 10 SDK
- [ ] Update NuGet packages:
  ```
  Umbraco.Cms                    13.2.2  --> 17.x.x
  uSync                          13.1.3  --> 17.x.x
  Our.Umbraco.FullTextSearch     4.1.0   --> 17.0.1
  Anthropic.SDK                  5.4.2   --> verify .NET 10 compat
  Microsoft.ICU.ICU4C.Runtime    68.2.0.9 --> check if still needed
  ```
- [ ] Remove `<RazorCompileOnBuild>false</RazorCompileOnBuild>` from csproj
- [ ] Remove `<RazorCompileOnPublish>false</RazorCompileOnPublish>` from csproj

### PHASE 2: Startup/Program.cs Changes

- [ ] Remove `u.UseInstallerEndpoints();` from `Startup.cs`
- [ ] Update middleware pipeline for v14+ patterns
- [ ] Check `MediaFileMiddleware` compatibility
- [ ] Verify Serilog configuration still works

### PHASE 3: View & Frontend Changes

- [ ] Remove Smidge references from `_ViewImports.cshtml`:
  - Remove `@addTagHelper *, Smidge`
  - Remove `@inject Smidge.SmidgeHelper SmidgeHelper`
- [ ] Update `MasterPage.cshtml` DOCTYPE to HTML5
- [ ] Delete all Grid partial views (`Views/Partials/grid/`)
- [ ] Update deprecated `Html.Partial()` calls to `Html.PartialAsync()`
- [ ] Verify TinyMCE content migrated correctly to TipTap

### PHASE 4: ModelsBuilder Regeneration

- [ ] Delete ALL `.generated.cs` files in `Interon.Roadlab.Web.Net.Core/Models/ContentModels/`
- [ ] Build and run the project to regenerate models
- [ ] Replace any `IPublishedSnapshot` references with `IPublishedContentCache` / `IPublishedMediaCache`
- [ ] Fix any compilation errors from API changes

### PHASE 5: uSync & Content Verification

- [ ] Update uSync to v17
- [ ] Re-export all uSync files
- [ ] Verify content synchronization works
- [ ] Check that date fields migrated to UTC correctly
- [ ] Test RTE content rendering (TipTap migration)

### PHASE 6: Testing

- [ ] Test all page templates render correctly
- [ ] Test all forms (Contact, Modal Contact, Vacancy)
- [ ] Test search functionality (FullTextSearch)
- [ ] Test account area (login, registration, transactions)
- [ ] Test media/image rendering
- [ ] Test backoffice login and content editing
- [ ] Test on mobile devices
- [ ] Verify Azure deployment pipeline works with .NET 10

---

## PRIORITY SUMMARY

### Do Now (Critical)
1. Rotate exposed credentials in appsettings.json
2. Fix file upload vulnerability in VacancySurfaceController
3. Remove debugger statements from production code
4. Remove/replace material.css (3.9 MB)

### Do Before Upgrade (High)
5. Migrate Nested Content to Block List (14 usages)
6. Migrate Grid Layout to Block Grid (1 usage)
7. Migrate legacy Media Picker to MediaPicker3 (12 usages)
8. Extract email service from duplicate controller code
9. Fix broken Azure pipeline

### Do During Upgrade (Required)
10. Update .NET 8 to .NET 10
11. Update all NuGet packages to v17
12. Regenerate ModelsBuilder models
13. Remove Smidge references
14. Update Startup.cs pipeline

### Do After Upgrade (Important)
15. Upgrade Bootstrap 4.3.1 to 5.x
16. Implement CSS bundling/minification
17. Move hardcoded content to CMS
18. Add canonical tags and structured data
19. Replace Revolution Slider with lightweight alternative
20. Add security headers middleware
21. Replace deprecated Razor helpers with tag helpers

---

*This document will be used as our working checklist. Items can be tackled in the priority order listed above.*
