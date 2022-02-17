using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPoco;

namespace Interon.Roadlab.Web.App.Models
{
    [NPoco.TableName("TransactionLines")]
    [PrimaryKey("Key", AutoIncrement = false)]
    public class TransactionLine

    {
       
       
        public Guid Key { get; set; }

        public string Value { get; set; }
        public string Category { get; set; }
        public int Qty { get; set; }
        
        public Guid TransactionKey { get; set; }
        public int CategoryId { get; set; }
        public int TestId { get; set; }
    }

}