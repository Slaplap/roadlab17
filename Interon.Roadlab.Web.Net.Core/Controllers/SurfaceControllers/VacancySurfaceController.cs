using System.Net;
using System.Net.Mail;
using System.Text;
using Interon.Roadlab.Web.Net.Core.Config;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Interon.Roadlab.Web.Net.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class VacancySurfaceController : SurfaceController
    {
        private readonly IOptions<EmailSettings> _emailSetings;
        private readonly ISpamFilterService _spamFilterService;
        private readonly ILogger<VacancySurfaceController> _logger;

        public VacancySurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider,IOptions<EmailSettings> emailSetings, ISpamFilterService spamFilterService, ILogger<VacancySurfaceController> logger) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailSetings = emailSetings;
            _spamFilterService = spamFilterService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> HandleSubmit(VacancyModel model, IFormFileCollection  files)
        {

            ModelState.Remove("Files");
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
                    "Website Vacancy Application",
                    model.Email ?? "unknown@domain.com",
                    $"Name: {model.Name}\nContact: {model.ContactNumber}\nVacancy Application"
                );

                if (spamResult.IsSpam && spamResult.ConfidenceScore > 80)
                {
                    TempData["Result"] = "Thank you for your application. We have received your submission and will review it shortly.";
                    return RedirectToCurrentUmbracoPage();
                }
                else if (spamResult.IsSpam && spamResult.ConfidenceScore >= 60)
                {
                    spamPrefix = "[SPAM] ";
                }
            }
            catch (Exception ex)
            {
                // Spam check failures must not block a legitimate application — log and continue.
                _logger.LogWarning(ex, "Spam analysis failed for vacancy submission; allowing through.");
            }

            const long maxFileSize = 5 * 1024 * 1024;
            var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".pdf", ".doc", ".docx" };

            string fileNameAndPath = "";
            IFormFile? file = files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Files", "Attachment must be 5MB or smaller.");
                    return RedirectToCurrentUmbracoPage();
                }

                var extension = Path.GetExtension(file.FileName);
                if (!allowedContentTypes.Contains(file.ContentType) || !allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Files", "Only PDF and Word documents (.pdf, .doc, .docx) are accepted.");
                    return RedirectToCurrentUmbracoPage();
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "VacancyUploads");
                Directory.CreateDirectory(uploadsFolder);

                string safeFileName = $"{Guid.NewGuid()}{extension}";
                fileNameAndPath = Path.Combine(uploadsFolder, safeFileName);
                using (var stream = new FileStream(fileNameAndPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

    
            try
            {
                //getting useful configuration
                string smtpAddress = _emailSetings.Value.SmtpSettings.Host;
                //it can be a "smtp.office365.com" or whatever,
                //it depends on smtp server of your sender email.
                int portNumber = _emailSetings.Value.SmtpSettings.Port; 
                bool enableSSL  = _emailSetings.Value.SmtpSettings.EnableSSL;
                // string emailTo = "marelize@lohansafaris.com";
                List<string> mailto = _emailSetings.Value.MailTo.Split(';').ToList<string>();

                string subject = spamPrefix + "Website Vacancy Form";

                StringBuilder body = new StringBuilder();

                //building the body of our email
                body.Append("<html><head> </head><body>");
                body.Append("<div style=' font-family: Arial;font - size: 14px; color: black; '>Web vacancy enquiry,<br><br>");
                body.Append(Request.Form["message"]);
                body.Append("</div><br>");
                //Mail signature
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>{0} </span><br>", model.Name));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Mail: <a href=\"mailto:{0}\">{0}</a></ span ><br> ", model.Email));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Tel: {0}</span><br>", model.ContactNumber));
                
                body.Append("</body></html>");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress( _emailSetings.Value.MailFrom);
                    //destination adress
                    foreach (var item in mailto)
                    {
                        mail.To.Add(item);
                    }

                   

                    mail.Subject = subject;
                    mail.Body    = body.ToString();
                    //set to true, to specify that we are sending html text.
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    if (file != null && !string.IsNullOrEmpty(fileNameAndPath))
                    {
                        mail.Attachments.Add(new Attachment(fileNameAndPath, file.ContentType));
                    }

                    //Specify the smtp Server and port number to create a new instance of SmtpClient.
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        //passing the credentials for authentication
                        smtp.Credentials = new NetworkCredential
                            (_emailSetings.Value.MailFrom,_emailSetings.Value.SmtpSettings.Password);
                        //Authentication required
                        smtp.EnableSsl = enableSSL;
                        //sending email.
                        smtp.Send(mail);
                    }
                }
                TempData["Result"] = "Thanks for your enquiry a consultant will be contacting you shortly";
                TempData["script"] = "document.getElementById(\"ContactFormPlaceHolder\").scrollIntoView();";
            }
            catch (Exception ex)
            {
                //Error response
                Response.StatusCode = 400;
                TempData["Result"]  = ex.Message;
            }
      


            return RedirectToCurrentUmbracoPage();
        }

    }
}