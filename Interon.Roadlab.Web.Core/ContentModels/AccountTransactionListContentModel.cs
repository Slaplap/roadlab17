using System.Collections.Generic;
using Interon.Roadlab.Web.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.Core.ContentModels
{
    public class AccountTransactionListContentModel : ContentModel
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<DropdownModel> Dropdown { get; set; } = new List<DropdownModel>();
        public AccountTransactionListContentModel(IPublishedContent content) : base(content)
        {
        }
    }
}