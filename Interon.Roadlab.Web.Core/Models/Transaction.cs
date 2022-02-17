using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NPoco;

namespace Interon.Roadlab.Web.App.Models
{
    [NPoco.TableName("Transactions")]
    [PrimaryKey("Key", AutoIncrement = false)]
  
    public class Transaction:Syncable

    {
        public Guid Key { get; set; }
        public string Number { get; set; }
        public string CompanyAccountNumber { get; set; }
        public string CompanyName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Project { get; set; }
        public int RequestedById { get; set; }
        public string RequestedByName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmail { get; set; }
        public string SiteLocationAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        //only ignore when creating table
        //[Ignore]
        public List<TransactionLine> TransactionLine { get; set; } = new List<TransactionLine>();
        //[Ignore]
        public List<TransactionFile> TransactionFile { get; set; } = new List<TransactionFile>();
        public string Comments { get; set; }
        public string Status { get; set; }
        public string OrderNumber { get; set; }
    }

    public class Syncable
    {
      
        
        public DateTime SyncDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}