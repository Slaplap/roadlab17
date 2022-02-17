using System.Collections.Generic;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.App.ViewModels
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