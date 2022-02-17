using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Interon.Roadlab.Web.Core.Controllers
{
    [Tree("Transactions", "Transactions", IsSingleNodeTree = true, TreeTitle = "Transactions",
        TreeGroup = "transactionsGroup", SortOrder = 5)]
    [PluginController("Transactions")]
    public class TransactionsTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
           return new TreeNodeCollection();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
             return new MenuItemCollection();
        }

        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            TreeNode root = CreateTreeNode("-1", "-1", queryStrings, "Transactions", "icon-chart-curve", false);

            root.RoutePath = $"Transactions/{TreeAlias}/transactions";

            return root;
        }
    }
}