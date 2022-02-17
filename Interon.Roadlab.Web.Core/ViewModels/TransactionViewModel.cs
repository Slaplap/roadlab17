using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class TransactionViewModel
    {
        

        public Guid Key { get; set; }
        public string CompanyAccountNumber { get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]

        public string ContactPerson { get; set; }
        [Required]
        public string ContactPersonNumber { get; set; }
        [Required]
        [EmailAddress]
        public string ContactPersonEmail { get; set; }
        [Required]
        public string SiteLocationAddress { get; set; }

        public string SpecialInstructions { get; set; }
        public List<TransactionLineViewModel> TransactionLine { get; set; } = new List<TransactionLineViewModel>();
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
     
        public string Number { get; set; }
        public int RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime SyncDate { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string OrderNumber { get; set; }
        public string comments;
    }
}