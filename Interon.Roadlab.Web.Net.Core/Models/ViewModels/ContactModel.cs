using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
    public class ContactModalModel
    {

        public string? BranchName { get; set; }
        public int BranchNumber { get; set; }
        public string? BranchEmail { get; set; }
        [Required]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Contact Number", Prompt = "Contact Number")]
        public string? ContactNumber { get; set; }
        [Required]
        [Display(Name = "Company", Prompt = "Company")]
        public string? Company { get; set; }
        [Required]
        [Display(Name = "Location", Prompt = "Location")]
        public string? Location { get; set; }
        [Required]
        [Display(Name = "Query", Prompt = "Query")]
        public string? Query { get; set; }
        [Required]
        [Display(Name = "I agree to the Terms and Conditions")]
        public bool AcceptTerms { get; set; }
    }
    /// <summary>
    /// Summary description for ContactModel
    /// </summary>
    public class ContactModel
    {
       
        public string? BranchName { get; set; } 
        public int BranchNumber { get; set; }   
        public string? BranchEmail { get; set; }  
        [Required]
        [Display(Name = "Name",Prompt = "Name")]
        public string Name { get; set; }
   
        [Required][EmailAddress]
        [Display(Name = "Email",Prompt = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display (Name= "Contact Number",Prompt = "Contact Number")]
        public string? ContactNumber { get; set; }
        [Required]
        [Display(Name = "Company", Prompt = "Company")]
        public string? Company { get; set; }
        [Required]
        [Display(Name = "Location", Prompt = "Location")]
        public string? Location { get; set; }
        [Required]
        [Display(Name = "Query", Prompt = "Query")]
        public string? Query { get; set; }
        [Required]
        [Display(Name = "I agree to the Terms and Conditions")]
        public bool AcceptTerms { get; set; }
               
    }
}