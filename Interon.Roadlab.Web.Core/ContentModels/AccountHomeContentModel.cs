using System.Collections.Generic;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.App.ContentModels
{
    public class AccountHomeContentModel : ContentModel
    {
        public List<Notification> Notifications { get; set; } = new List<Notification>();

        public AccountHomeContentModel(IPublishedContent content) : base(content)
        {
        }
    }
}