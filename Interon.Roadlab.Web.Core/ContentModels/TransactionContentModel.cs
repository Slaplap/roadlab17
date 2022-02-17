using System.Collections.Generic;
using Interon.Roadlab.Core.Json;
using Interon.Roadlab.Web.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Interon.Roadlab.Web.Core.ContentModels
{
    public class TransactionContentModel : ContentModel
    {
        public Transaction Transaction { get; set; }
        
        public List<Comment> Comments = new List<Comment>();
      
        public TransactionContentModel(IPublishedContent content) : base(content)
        {
           
        }
    }
}