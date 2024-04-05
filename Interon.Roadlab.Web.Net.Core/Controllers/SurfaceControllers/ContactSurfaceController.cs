using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using Azure;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;

namespace Interon.Roadlab.Web.Net.Core.Controllers.SurfaceControllers
{
    public class ConfigurationSMTP
    {
        //SMTP parameters
        public static string smtpAdress = ConfigurationManager.AppSettings["smtpAddress"]?? "mail.roadlab.co.za";
        public static int portNumber = 587;
        public static bool enableSSL = false;
        //need it for the secured connection
        public static string from = System.Configuration.ConfigurationManager.AppSettings["mailfrom"];
        public static string password = "54SqQseRgZjQT2d5";
    }
    /// <summary>
    /// Summary description for ContactSurfaceController
    /// </summary>
    public class ContactSurfaceController : Umbraco.Cms.Web.Website.Controllers.SurfaceController
    {
        [HttpPost]
        public IActionResult HandleSubmit(ContactModel model)
        {


            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage();
            }
            //honeypot
            if (!string.IsNullOrEmpty(model.Surname))
            {
                TempData["Result"] = "Thank you for your enquiry. A consultant will be contacting you shortly.";
                TempData["script"] = "document.getElementById(\"ContactFormPlaceHolder\").scrollIntoView();";
                return RedirectToCurrentUmbracoPage();
            }

            try
            {
                //getting useful configuration
                string smtpAddress = ConfigurationSMTP.smtpAdress;
                //it can be a "smtp.office365.com" or whatever,
                //it depends on smtp server of your sender email.
                int portNumber = ConfigurationSMTP.portNumber;   //Smtp port
                bool enableSSL = ConfigurationSMTP.enableSSL;  //SSL enable
                // string emailTo = "marelize@lohansafaris.com";
                List<string> mailto = System.Configuration.ConfigurationManager.AppSettings["mailto"].Split(';').ToList<string>();

                string subject = "Website Contact Form - " + @model.BranchName; 

                StringBuilder body = new StringBuilder();

                //building the body of our email
                body.Append("<html><head> </head><body>");
                body.Append("<div style=' font-family: Arial;font - size: 14px; color: black; '>Web enquiry,<br><br>");
                body.Append(Request.Form["message"]);
                body.Append("</div><br>");
                //Mail signature
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>{0} </span><br>", model.Name));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Mail: <a href=\"mailto:{0}\">{0}</a></ span ><br> ", model.Email));
                body.Append(string.Format("<span style='font-size:11px;font-family:Arial; color:#40411E;'>Tel: {0}</span><br>", model.ConactNumber));
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
                            (ConfigurationSMTP.from, ConfigurationSMTP.password);
                        //Authentication required
                        smtp.EnableSsl = enableSSL;
                        //sending email.
                        smtp.Send(mail);
                    }
                }
                TempData["Result"] = "Thanks for your enquiry a consultant will be conting you shortly";
                TempData["script"] = "document.getElementById(\"ContactFormPlaceHolder\").scrollIntoView();";
            }
            catch (Exception ex)
            {
                //Error response
                Response.StatusCode = 400;
                TempData["Result"] = ex.Message;
            }
      


            return RedirectToCurrentUmbracoPage();
        }

        public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }
    }
}