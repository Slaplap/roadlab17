using System.Net;
using System.Net.Mail;
using System.Text;
using Interon.Roadlab.Web.Net.Core.Config;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Interon.Roadlab.Web.Net.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Interon.Roadlab.Web.Net.Core.Controllers.SurfaceControllers
{
    /// <summary>
    /// Summary description for ContactSurfaceController
    /// </summary>
    public class ModalContactSurfaceController : SurfaceController
    {
        private readonly IOptions<EmailSettings> _mailsettings;
        private readonly ISpamFilterService _spamFilterService;

        [HttpPost]
        public async Task<IActionResult> HandleSubmit(ContactModel model)
        {


            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage();
            }

            if (!model.AcceptTerms)
            {
                ModelState.AddModelError("AcceptTerms", "You must accept the Terms and Conditions to submit this form.");
                return RedirectToCurrentUmbracoPage();
            }

            // Spam detection
            string spamPrefix = "";
            try
            {
                var spamResult = await _spamFilterService.AnalyzeEmailAsync(
                    "Website Modal Contact Form",
                    model.Email ?? "unknown@domain.com",
                    $"Name: {model.Name}\nCompany: {model.Company}\nLocation: {model.Location}\nQuery: {model.Query}"
                );

                Serilog.Log.Information("Spam analysis completed for modal contact form. Classification: {Classification}, Confidence: {Confidence}, Sender: {Email}", 
                    spamResult.Classification, spamResult.ConfidenceScore, model.Email);

                if (spamResult.IsSpam && spamResult.ConfidenceScore > 80)
                {
                    Serilog.Log.Warning("Modal contact form submission blocked as spam. Email: {Email}, Reason: {Reasoning}", 
                        model.Email, spamResult.Reasoning);
                    
                    TempData["Result"] = "Thank you for your message. We have received your enquiry and will respond shortly.";
                    return RedirectToCurrentUmbracoPage();
                }
                else if (spamResult.IsSpam && spamResult.ConfidenceScore >= 60)
                {
                    Serilog.Log.Warning("Modal contact form flagged as potential spam. Email: {Email}, Confidence: {Confidence}, Reason: {Reasoning}", 
                        model.Email, spamResult.ConfidenceScore, spamResult.Reasoning);
                    
                    spamPrefix = "[SPAM] ";
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning(ex, "Spam filter failed for modal contact form, allowing submission to proceed. Email: {Email}", model.Email);
            }

            try
            {
                //getting useful configuration
                string smtpAddress =  _mailsettings.Value.SmtpSettings.Host;
                //it can be a "smtp.office365.com" or whatever,
                //it depends on smtp server of your sender email.
                int portNumber = _mailsettings.Value.SmtpSettings.Port;   //Smtp port
                bool enableSSL =  _mailsettings.Value.SmtpSettings.EnableSSL;  //SSL enable
                // string emailTo = "marelize@lohansafaris.com";
                List<string> mailto =   _mailsettings.Value.MailTo.Split(';').ToList<string>();

                string subject = spamPrefix + "Website Enquiry for Mozambique - "  ;

                StringBuilder body = new StringBuilder();

                //building the body of our email
                body.Append("<html><head> </head><body>");
                body.Append("<div style=' font-family: Arial;font - size: 14px; color: black; '>Web enquiry,<br><br>");
                body.Append(Request.Form["message"]);
                body.Append("</div><br>");
                //Mail signature
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>{0} </span><br>", model.Name));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Mail: <a href=\"mailto:{0}\">{0}</a></ span ><br> ", model.Email));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Tel: {0}</span><br>", model.ContactNumber));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Company: {0}</span><br>", model.Company));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Location: {0}</span><br>", model.Location));
                body.Append(string.Format("<span style='font-size:11px; font-family:Arial; color:#40411E;'>{0}</span><br>", model.Query));
                body.Append(string.Format("<span style='font-size:11px; font-family:Arial; color:#40411E;'>{0}</span><br>", model.BranchEmail));
                body.Append("</body></html>");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["mailfrom"]);
                    //destination adress
                    foreach (var item in mailto)
                    {
                        mail.To.Add(item);
                    }

                    if (!string.IsNullOrWhiteSpace(model.BranchEmail))
                    {
                        mail.To.Add(model.BranchEmail);
                    }

                    mail.Subject = subject;
                    mail.Body = body.ToString();
                    //set to true, to specify that we are sending html text.
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    string localFileName = "~/Content/TestAttachement.txt";
                    //to send a file in attachment.
                    //mail.Attachments.Add(new Attachment
                    //(Server.MapPath(localFileName), "application/pdf"));

                    //Specify the smtp Server and port number to create a new instance of SmtpClient.
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        //passing the credentials for authentication
                        smtp.Credentials = new NetworkCredential
                            (_mailsettings.Value.MailFrom,_mailsettings.Value.SmtpSettings.Password);
                        //Authentication required
                        smtp.EnableSsl = enableSSL;
                        //sending email.
                        smtp.Send(mail);
                    }
                }
                TempData["Result"] = "Thanks for your enquiry a consultant will be contacting you shortly";
                TempData["script"] = "setTimeout(function(){ $(document).ready(function(){ $('#mozModal').modal('show');});  }, 500);";
            }
            catch (Exception ex)
            {
                //Error response
                Response.StatusCode = 400; 
                TempData["Result"] = ex.Message;
            }



            return RedirectToCurrentUmbracoPage();
        }

        public ModalContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider,IOptions<EmailSettings> mailsettings, ISpamFilterService spamFilterService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _mailsettings = mailsettings;
            _spamFilterService = spamFilterService;
        }
    }
}

