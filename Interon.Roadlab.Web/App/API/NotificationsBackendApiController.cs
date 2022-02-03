using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.App.Models;
using Interon.Roadlab.Web.App.Services;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services.Implement;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Member = Umbraco.Web.PublishedModels.Member;
using NotificationService = Interon.Roadlab.Web.App.Services.NotificationService;

namespace Interon.Roadlab.Web.App.API
{
    [PluginController("NotificationsBackendApi")]
    public class NotificationsBackendApiController : UmbracoAuthorizedJsonController
    {
        private NotificationService _NotificationService;

        public NotificationsBackendApiController()
        {
        }
        public NotificationsBackendApiController(NotificationService NotificationService)
        {
            _NotificationService = NotificationService;
        }
        [System.Web.Http.HttpGet]
        public List<Notification> GetAllNotifications()
        {
            return new List<Notification>();
        }
        
     

        [System.Web.Http.HttpGet]
        public NotificationJsonDto GetNotificationById(Guid notificationKey)
        {

            var notification = _NotificationService.GetNotificationsByKey(notificationKey);

            return  _NotificationService.NotificationToDto(notification);
        }


        [System.Web.Http.HttpPost]
        public System.Web.Http.Results.JsonResult<string> UploadFile(FormDataCollection formData, string memberId)
        {
            
            return Json("false");
        }
       
        [System.Web.Http.HttpGet]
        public PagedUmbracoResult GetPaged(string itemsPerPage, string pageNumber, string sortColumn, string sortOrder, string status, string searchTerm)
        {
           var result = _NotificationService.GetPaged(int.Parse(itemsPerPage), int.Parse(pageNumber), sortColumn, sortOrder, status, searchTerm);
           return result;
        }



    }
}