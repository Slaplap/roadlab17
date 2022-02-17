using System;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class TransactionLineViewModel
    {

        
        public int CategoryId { get; set; }
        public int TestId { get; set; }
        public int Qty { get; set; }
        public string Category { get; set; }
        public string Test { get; set; }
        public Guid Key { get; set; }
        public Guid TransactionKey { get; set; }
        public string Value { get; set; }
    }
}