using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.Services;
using Interon.Roadlab.Web.App.Models;
using Interon.Roadlab.Web.App.Services;
using Microsoft.Azure.NotificationHubs;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services.Implement;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Member = Umbraco.Web.PublishedModels.Member;
using Notification = Interon.Roadlab.Web.App.Models.Notification;
using NotificationService = Interon.Roadlab.Web.App.Services.NotificationService;

namespace Interon.Roadlab.Web.App.API
{
    [PluginController("TransactionsBackendApi")]
    public class TransactionsBackendApiController : UmbracoAuthorizedJsonController
    {
        //private TransactionService _serviceRequestService;
        //private NotificationService _notificaitonService;

        //public TransactionsBackendApiController()
        //{
        //}
        //public TransactionsBackendApiController(ServiceRequestService serviceRequestServiceRequestServiceRequestService, NotificationService notificatiodService)
        //{
        //    _serviceRequestService = serviceRequestServiceRequestServiceRequestService;
        //    _notificaitonService = notificatiodService;
        //}
        //[System.Web.Http.HttpGet]
        //public List<Transaction> GetAllTransactions()
        //{
        //    return new List<Transaction>();
        //}
        //[System.Web.Http.HttpGet]
        //public async Task<RootClientRequest> SaveStatusById(Guid transactionKey, string status)
        //{
        //    try
        //    {

        //        var transaction = _serviceRequestService.GetTransactionAndLinesByKey(transactionKey);
        //        var _member = Umbraco.Member(transaction.RequestedById);
        //        Member m = new Member(_member);

        //        if (transaction != null)
        //        {

        //            transaction.Status = status;
        //            _serviceRequestService.SaveTransactionAndTransactionLines(transaction, false, true);
        //        }


        //        var Title = "";
        //        var Message = "";

        //        if (transaction.Status.Contains(TransactionStatus.Open))
        //        {
        //            Title = "Request Received";
        //            Message = "Your request has been received and will be processed.";
        //        }
        //        else if (transaction.Status.Contains(TransactionStatus.Ready))

        //        {
        //            Title = "Action Required";
        //            Message = "Please accept or reject our response to your request by clicking the eye icon";
        //        }
        //        else
        //        {
        //            Title = "Update";
        //            Message = $"Your request status has moved to :{transaction.Status}";
        //        }



        //        var timespan = DateTime.Now - m.CommunicationDateTime;

        //        if (m != null && !Request.RequestUri.Host.Contains("localhost") && timespan.TotalMinutes > 5)
        //        {

        //            SmsService.SendSMS(m.MobileNumber, "Roadlab - " + Title + " : " + Message);

        //        }

        //        NotificationOutcome outcome = null;
        //        if (timespan.TotalMinutes < 5)
        //        {
                  
        //            outcome = await NotificationHubService.Instance.SendAndroidNotification(m.MobileNumber, Message, "fcm", "Webserver", "RoadlabNotificationHub");
        //        }

        //        if (outcome == null)
        //            {

        //                SmsService.SendSMS(m.MobileNumber, "Roadlab - " + Title + " : " + Message);
        //            }
        //            _notificaitonService.CreateOrUpdateNotification(new Notification()
        //            {
        //                CreateDate = DateTime.Now,
        //                Data = "",
        //                Key = Guid.NewGuid(),
        //                Link = $"TransactionPage?TransactionKey={transaction.Key}",
        //                MemberId = m.Id,
        //                Message = Message,
        //                ReadDate = null,
        //                SyncDate = DateTime.Now,
        //                Title = Title,
        //                ForeignKey = transaction.Key,
        //                PushOutcome = outcome?.State.ToString() ?? "None",
        //                NotificationType = "Transaction"


        //            }, true, true);
                
        //        return _serviceRequestService.TransactionToDto(transaction);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //[System.Web.Http.HttpGet]
        //public TransactionJsonDto GetTransactionById(Guid transactionKey)
        //{

        //    var transaction = _serviceRequestService.GetTransactionAndLinesByKey(transactionKey);

        //    return _serviceRequestService.TransactionToDto(transaction);
        //}


        //[System.Web.Http.HttpPost]
        //public System.Web.Http.Results.JsonResult<string> UploadFile(FormDataCollection formData, string memberId)
        //{

        //    return Json("false");
        //}

        //[System.Web.Http.HttpGet]
        //public PagedUmbracoResult GetPaged(string itemsPerPage, string pageNumber, string sortColumn, string sortOrder, string status, string searchTerm)
        //{
        //    var result = _serviceRequestService.GetPaged(int.Parse(itemsPerPage), int.Parse(pageNumber), sortColumn, sortOrder, status, searchTerm);
        //    return result;
        //}



    }
}