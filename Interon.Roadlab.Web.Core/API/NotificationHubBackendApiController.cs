using System.Web.Http;
using Interon.Roadlab.Web.Core.ContentModels;
using Interon.Roadlab.Web.Core.Models;
using Interon.Roadlab.Web.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using NotificationService = Interon.Roadlab.Web.Core.Services.NotificationService;

namespace Interon.Roadlab.Web.Core.API
{
    [PluginController("NotificationSender")]
    public class NotificationHubBackendApiController : UmbracoAuthorizedJsonController
    {
       
        private IMembershipService _membershipService;
        private NotificationService _notificationService;

        [HttpGet]
        public bool test()
        {
            return true;
        }
        public NotificationHubBackendApiController( IMembershipService membershipMemberService, NotificationService notificatiodService)
        {
           
            _membershipService = membershipMemberService;
            _notificationService = notificatiodService;
        }
        public NotificationHubBackendApiController()
        {
            
        }



        [HttpPost]
        public void SendNotificationToMember(MyClass mc)
        {
             
           
            var m = (Member) Umbraco.MembershipHelper.GetById(mc.memberId);
            _notificationService.CreateOrUpdateNotification(new Notification()
            {
                Message = mc.message,
                Title   = "",
            }, true, true);
            NotificationHubService.Instance.SendAndroidNotification(m.MobileNumber, mc.message, "fcm", "Webserver", "RoadlabNotificationHub");
        }



    }

    public class MyClass
    {
        public int memberId { get; set; }
        public string message { get; set; }
    }
}