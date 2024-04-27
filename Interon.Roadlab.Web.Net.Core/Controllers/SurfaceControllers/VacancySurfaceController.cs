using System.Net;
using System.Net.Mail;
using System.Text;
using Interon.Roadlab.Web.Net.Core.Config;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Microsoft.AspNetCore.Http;
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
    public class VacancySurfaceController : SurfaceController
    {
        private readonly IOptions<EmailSettings> _emailSetings;
        public VacancySurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider,IOptions<EmailSettings> emailSetings) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailSetings = emailSetings;
        }

        [HttpPost]
        public IActionResult HandleSubmit(VacancyModel model, IFormFileCollection  files)
        {

            ModelState.Remove("Files");
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage();
            }

         


            string fileNameAndPath = "";
            IFormFile? file = files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                fileNameAndPath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(fileNameAndPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                // Handle the case when no file is uploaded
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

                string subject = "Website Vacancy Form";

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

                    string localFileName = fileNameAndPath;
                    //to send a file in attachment.
                    mail.Attachments.Add(new Attachment(localFileName,  file.ContentType));

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