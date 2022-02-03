using System.Collections.Generic;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class AccountTransactionCreateContentModel : ContentModel
    {
        
        public AccountTransactionCreateContentModel(IPublishedContent content) : base(content)
        {
        }
    }
    public class AccountLogin : ContentModel
    {

        public AccountLogin(IPublishedContent content) : base(content)
        {
        }
    }
}