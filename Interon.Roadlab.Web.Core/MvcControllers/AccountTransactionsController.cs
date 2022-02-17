using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interon.Roadlab.Web.App.Models;
using Interon.Roadlab.Web.App.Services;
using Interon.Roadlab.Web.App.ViewModels;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Interon.Roadlab.Web.App.mvcControllers
{

    public class AccountTransactionListController : RenderMvcController
    {
        
        private IMembershipService _membershipsService;
        private UmbracoHelper _umbracoHelper;

        public AccountTransactionListController(IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, UmbracoHelper umbracoHelper,IMembershipService membershipService) : base(globalSettings, umbracoContextAccessor, services, appCaches, profilingLogger, umbracoHelper)
        {
            
            _membershipsService = membershipService;
            _umbracoHelper = umbracoHelper;
        }

        public override ActionResult Index(ContentModel model)
        {
            AccountTransactionListContentModel rm = new AccountTransactionListContentModel(model.Content);
            //List<Transaction> transaction;
            //if (Request.QueryString["type"] == "My")
            //{
            //transaction = _transactionService.GetTransactionsByClientAndContact(_membershipsService.GetCurrentMemberClients().FirstOrDefault().AccountNumber).Where(x=>x.RequestedById == Umbraco.MembershipHelper.GetCurrentMemberId()).OrderByDescending(x => x.UpdateDate).ToList();

            //}
            //transaction = _transactionService.GetTransactionsByClientAndContact(_membershipsService.GetCurrentMemberClients().FirstOrDefault().AccountNumber).OrderByDescending(x => x.UpdateDate).ToList();
            //rm.Transactions = transaction;
            //rm.Dropdown.Add(new DropdownModel() { Text = "Company Transactions", Value = "Company" });
            //rm.Dropdown.Add(new DropdownModel() { Text = "My Transactions", Value      = "My" });
            return base.Index(rm);
        }
    }
}
