using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Interon.Roadlab.Web.Core.API
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