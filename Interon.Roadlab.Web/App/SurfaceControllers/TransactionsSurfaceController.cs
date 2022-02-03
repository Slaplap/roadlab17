using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Services;
using Interon.Roadlab.Web.App.ViewModels;
using Microsoft.Azure.NotificationHubs;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using Notification = Interon.Roadlab.Web.App.Models.Notification;

namespace Interon.Roadlab.Web.App.SurfaceControllers
{
    public class TransactionsSurfaceController : SurfaceController
    {
        private readonly ServiceContext _services;
        
        private IMembershipService _membershipService;
       
        private NotificationService _notificationService;

        public TransactionsSurfaceController(IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, ILogger logger,
            IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper, IMembershipService membershipService,  NotificationService notificationService) : base(umbracoContextAccessor,
            databaseFactory, services, appCaches, logger, profilingLogger, umbracoHelper)
        {
            _services = services;
          
            _membershipService = membershipService;
           
            _notificationService = notificationService;
        }



        [HttpPost]
        [ActionName("Confirmation")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation(ConfirmationViewModel model)
        {
            //  if (!ModelState.IsValid) return CurrentUmbracoPage();


            return Redirect("/account/secure-area/List?type=All");
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Create(TransactionViewModel model)
        {
         //   if (!ModelState.IsValid) return CurrentUmbracoPage();
         //   if (!model.TransactionLine.Any())
         //   {
         //       ModelState.AddModelError("", "Please select service by clicking the plus icon");
         //       return CurrentUmbracoPage();
         //   }

         //   var member = (Member)Members.GetCurrentMember();
         //   var type = Request.QueryString["type"] ?? TransactionStatus.Quote;
         //   model.Status = type + " " + TransactionStatus.Open;
         //   var branches = Umbraco.ContentAtRoot().OfType<Branches>().FirstOrDefault()?.Children.OfType<Branch>();
         //   model.BranchName = (branches ?? throw new InvalidOperationException()).FirstOrDefault(x => x.Id == model.BranchId)?.BranchName;
         //   model.RequestedBy = Members.GetCurrentMemberId();
         //   model.RequestedByName = member.FirstName + " " + member.Surname;

         // //  model.CompanyName = _companyService.GetCompanyById(member.Company).CompanyName;
         // //TODO add comppany 
         ////   model.CompanyAccountNumber = member.Company.Id;
         //   model.Key = Guid.NewGuid();
         //   model.Number = "";

         //   foreach (var transactionLineViewModel in model.TransactionLine)
         //   {
         //       transactionLineViewModel.Key = Guid.NewGuid();
         //       transactionLineViewModel.TransactionKey = model.Key;
         //       transactionLineViewModel.Category = Umbraco.ContentQuery.Content(transactionLineViewModel.CategoryId).Name;
         //       transactionLineViewModel.Test = Umbraco.ContentQuery.Content(transactionLineViewModel.TestId).Name;



         //   }
         //   var transaction = _transactionService.TransactionViewModelToTransaction(model);

         //   _transactionService.SaveTransactionAndTransactionLines(transaction, true, true);
         //   var message = _transactionService.TransactionMessage(transaction.Status);



         //   var timespan = DateTime.Now - member.CommunicationDateTime;
         //   if (!Request.Url.Host.Contains("localhost") && timespan.TotalMinutes > 5)
         //   {

         //       SmsService.SendSMS(member.MobileNumber, "Roadlab - " + message.Title + " : " + message.Body);
         //   }

         //   NotificationOutcome outcome = null;
         //   if (timespan.TotalMinutes < 5)
         //   {
         //       outcome = await NotificationHubService.Instance.SendAndroidNotification(member.MobileNumber, message.Title + ": " + message.Body, "fcm", "Webserver", "RoadlabNotificationHub").ConfigureAwait(true);
               
         //   }

         //   if (outcome == null&& !Request.Url.Host.Contains("localhost"))
         //   {

         //       SmsService.SendSMS(member.MobileNumber, "Roadlab - " + message.Title + " : " + message.Body);
         //   }
         //   _notificationService.CreateOrUpdateNotification(new Notification()
         //   {
         //       CreateDate = DateTime.Now,
         //       Data = "",
         //       Key = Guid.NewGuid(),
         //       Link = $"TransactionPage?TransactionKey={transaction.Key}",
         //       ForeignKey = transaction.Key,
         //       MemberId = member.Id,
         //       Message = message.Body,
         //       ReadDate = null,
         //       SyncDate = DateTime.Now,
         //       Title = message.Title,
         //       PushOutcome = outcome?.State.ToString() ?? "None",
         //       NotificationType = "Transaction"


         //   }, true, true);



            return Redirect("/account/secure-area/List?type=All");
        }
    }
}