using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Models;
using Interon.Roadlab.Web.App.Services;
using Interon.Roadlab.Web.Core.ContentModels;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services.Implement;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
 
using NotificationService = Interon.Roadlab.Web.App.Services.NotificationService;

namespace Interon.Roadlab.Web.App.API
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