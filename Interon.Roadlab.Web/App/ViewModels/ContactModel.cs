using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.UI.WebControls;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class ContactModalModel
    {

        public int BranchNumber { get; set; }

        public string BranchEmail { get; set; }
        [Required]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Contact Number", Prompt = "Contact Number")]
        public string ConactNumber { get; set; }
        [Required]
        [Display(Name = "Subject", Prompt = "Subject")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Query", Prompt = "Query")]
        public string Query { get; set; }
        //this is a honey pot field
        public string Surname { get; set; }
        public string BranchName { get; set; }
    }
    /// <summary>
    /// Summary description for ContactModel
    /// </summary>
    public class ContactModel
    {
       
        public string BranchName { get; set; }
        public int BranchNumber { get; set; }
        public string BranchEmail { get; set; }
        [Required]
        [Display(Name = "Name",Prompt = "Name")]
        public string Name { get; set; }
   
        [Required][EmailAddress]
        [Display(Name = "Email",Prompt = "Email")]
        public string Email { get; set; }
        [Required]
        [Display (Name= "Contact Number",Prompt = "Contact Number")]
        public string ConactNumber { get; set; }
        [Required]
        [Display(Name = "Subject", Prompt = "Subject")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Query", Prompt = "Query")]
        public string Query { get; set; }
        //this is a honey pot field
        public string Surname { get; set; }
    }
}