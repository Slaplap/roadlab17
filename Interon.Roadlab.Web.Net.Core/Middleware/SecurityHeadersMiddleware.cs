using Microsoft.AspNetCore.Http;

namespace Interon.Roadlab.Web.Net.Core.Middleware;

/// <summary>
/// Adds defence-in-depth response headers on every request:
///   X-Content-Type-Options: nosniff  — stops browsers MIME-sniffing scripts
///                                       out of non-script content.
///   X-Frame-Options:        SAMEORIGIN — prevents the site being framed by
///                                       other origins (clickjacking).
///   Referrer-Policy:        strict-origin-when-cross-origin — limits what
///                                       URL the browser leaks in Referer.
/// HSTS is wired up separately via `app.UseHsts()` so it only fires in
/// non-development environments.
/// CSP is intentionally not added here — to be introduced separately in
/// report-only mode first so we can see what breaks before enforcing.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;
        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "SAMEORIGIN";
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        return _next(context);
    }
}
