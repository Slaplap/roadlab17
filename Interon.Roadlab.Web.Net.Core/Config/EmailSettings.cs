namespace Interon.Roadlab.Web.Net.Core.Config;

public class EmailSettings
{
    public string MailTo { get; set; }
    public string MailFrom { get; set; }
    public SmtpSettings SmtpSettings { get; set; }
}