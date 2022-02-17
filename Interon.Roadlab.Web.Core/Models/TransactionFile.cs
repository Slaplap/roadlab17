using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPoco;

namespace Interon.Roadlab.Web.App.Models
{
    [NPoco.TableName("TransactionFiles")]
    public class TransactionFile
    {
        [Column("Key")]
        public Guid Key { get; set; }
        public Guid TransactionKey { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
    }
}