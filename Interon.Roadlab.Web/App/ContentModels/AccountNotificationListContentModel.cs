using System.Collections.Generic;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class AccountNotificationListContentModel : ContentModel
    {
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        
        public AccountNotificationListContentModel(IPublishedContent content) : base(content)
        {
        }
    }
}