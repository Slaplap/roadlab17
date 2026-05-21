namespace Interon.Roadlab.Web.Net.Core.Logging;

public static class PiiMasking
{
    /// <summary>
    /// Masks an email address for logging: keeps the first character of the
    /// local part plus the full domain, so we can still correlate entries
    /// without writing the full address. Returns "***" for malformed input
    /// and the original value if null or empty.
    /// </summary>
    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrEmpty(email)) return email ?? "";
        var at = email.IndexOf('@');
        if (at <= 0 || at == email.Length - 1) return "***";
        var local = email.Substring(0, at);
        var domain = email.Substring(at);
        return (local.Length <= 1 ? local : local[0] + "***") + domain;
    }
}
