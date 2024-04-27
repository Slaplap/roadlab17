using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using Interon.Roadlab.Web.Net.Core.Config;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;

namespace Interon.Roadlab.Web.Net.Core.Controllers.SurfaceControllers
{
     
    /// <summary>
    /// Summary description for ContactSurfaceController
    /// </summary>
    public class ContactSurfaceController : Umbraco.Cms.Web.Website.Controllers.SurfaceController
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IOptions<EmailSettings> emailSettings) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailSettings = emailSettings;
        }
         
        [HttpPost]
        public IActionResult HandleSubmit(ContactModel model)
        {


            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage();
            }
           

            try
            {

                //getting useful configuration
                string smtpAddress = _emailSettings.Value.SmtpSettings.Host;
                //it can be a "smtp.office365.com" or whatever,
                //it depends on smtp server of your sender email.
                int portNumber =  _emailSettings.Value.SmtpSettings.Port;   //Smtp port
                bool enableSSL = _emailSettings.Value.SmtpSettings.EnableSSL;  //SSL enable
                
                List<string> mailto = _emailSettings.Value.MailTo.Split(';').ToList<string>();

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
                Log.Information("Start Email Sending");
                Log.Information("Email To: {0}", _emailSettings.Value.MailTo);
                Log.Information("Email From: {0}", _emailSettings.Value.MailFrom);

                using (MailMessage mail = new MailMessage())
                {
                    try
                    {
                        mail.From = new MailAddress(_emailSettings.Value.MailFrom);
                        //destination address
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

                        //Specify the smtp Server and port number to create a new instance of SmtpClient.
                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            //passing the credentials for authentication
                            smtp.Credentials = new NetworkCredential(_emailSettings.Value.MailFrom, _emailSettings.Value.SmtpSettings.Password);
                            //Authentication required
                            smtp.EnableSsl = enableSSL;
                            //sending email.
                            smtp.Send(mail);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error sending email. Configuration: SmtpAddress: {0}, Port: {1}, EnableSSL: {2}, MailFrom: {3}, MailTo: {4}, Subject: {5}, Body: {6}",
                            smtpAddress, portNumber, enableSSL, _emailSettings.Value.MailFrom, string.Join(";", mailto), subject, body.ToString());
                    }
                }
                TempData["Result"] = "Thanks for your enquiry a consultant will be contacting you shortly";
                TempData["script"] = "document.getElementById(\"ContactFormPlaceHolder\").scrollIntoView();";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending email");
                Response.StatusCode = 400;
                TempData["Result"] = ex.Message;
            }
      


            return RedirectToCurrentUmbracoPage();
        }

       
    }
}