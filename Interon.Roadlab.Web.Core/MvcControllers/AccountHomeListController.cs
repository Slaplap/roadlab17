using System.Collections.Generic;
using System.Web.Mvc;
using Interon.Roadlab.Web.Core.ContentModels;
using Interon.Roadlab.Web.Core.Models;
using Interon.Roadlab.Web.Core.Services;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Interon.Roadlab.Web.Core.MvcControllers
{
    public class AccountHomeController : RenderMvcController
    {
        private NotificationService _notificaitonService;
        private IMembershipService _membershipsService;
        private UmbracoHelper _umbracoHelper;

        public AccountHomeController(IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper, NotificationService notificationService, IMembershipService membershipService) : base(globalSettings, umbracoContextAccessor, services, appCaches, profilingLogger, umbracoHelper)
        {
            _notificaitonService = notificationService;
            _membershipsService  = membershipService;
            _umbracoHelper       = umbracoHelper;
        }

        public override ActionResult Index(ContentModel model)
        {
            AccountHomeContentModel rm = new AccountHomeContentModel(model.Content);
            List<Notification>                  notifications;

            notifications    = _notificaitonService.GetNotificationsByMemberId(Members.GetCurrentMemberId());
            rm.Notifications = notifications;
            
            return base.Index(rm);
        }
    }
}