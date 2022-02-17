using System;
using NPoco;

namespace Interon.Roadlab.Web.Core.Models
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