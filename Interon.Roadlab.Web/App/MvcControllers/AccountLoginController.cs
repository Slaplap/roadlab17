using System.Web.Mvc;
using Interon.Roadlab.LIMS.Services;
using Interon.Roadlab.Web.App.Services;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Interon.Roadlab.Web.App.mvcControllers
{
    public class AccountLoginController : RenderMvcController
    {
      
        private IMembershipService _membershipsService;
        private UmbracoHelper _umbracoHelper;

        public AccountLoginController(IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper,  IMembershipService membershipService) : base(globalSettings, umbracoContextAccessor, services, appCaches, profilingLogger, umbracoHelper)
        {
        
            _membershipsService = membershipService;
            _umbracoHelper      = umbracoHelper;
        }

        public override ActionResult Index(ContentModel model)
        {
            TempData["RedirectUrl"] = Request.Url.AbsoluteUri;
            return base.Index(model);
        }
    }
}