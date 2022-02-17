using System.Web.Mvc;
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

    public class AjaxPageController : RenderMvcController
    {
       
        private IMembershipService _membershipsService;
        private UmbracoHelper _umbracoHelper;

        public AjaxPageController(IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper,IMembershipService membershipService) : base(globalSettings, umbracoContextAccessor, services, appCaches, profilingLogger, umbracoHelper)
        {
           
            _membershipsService = membershipService;
            _umbracoHelper = umbracoHelper;
        }

        public override ActionResult Index(ContentModel model)
        {
            //var key = Request.QueryString["key"].ToString();
            //TransactionContentModel rm = new TransactionContentModel(model.Content);
            //var transaction = _transactionService.GetTransactionAndLinesByKey(Guid.Parse(key));
            //if (!string.IsNullOrWhiteSpace(transaction.Comments))

            //{
            //    var comments = JsonConvert.DeserializeObject<Comments>(transaction.Comments);
            //    foreach (var comment in comments.CommentsList)
            //    {
            //        rm.Comments.Add(comment);
            //    }
            //}
          
            //rm.Transaction = transaction;
           return base.Index(model);
        }
    }
}