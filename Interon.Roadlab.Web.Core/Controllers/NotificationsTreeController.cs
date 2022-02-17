using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Interon.Roadlab.Web.App.Controllers
{
    [Tree("Notifications", "Notifications", IsSingleNodeTree = true, TreeTitle = "Notifications",
        TreeGroup = "NotificationsGroup", SortOrder = 5)]
    [PluginController("Notifications")]
    public class NotificationsTreeController : TreeController
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
            TreeNode root = CreateTreeNode("-1", "-1", queryStrings, "Notifications", "icon-chart-curve", false);

            root.RoutePath = $"Notifications/{TreeAlias}/Notifications";

            return root;
        }
    }
}