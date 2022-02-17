using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Prompt = "Email" )]
        
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter Name")]

        [Display(AutoGenerateField = false)]

        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Surname")]
        [Display(Prompt = "Surname" )]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter cell no")]
        [Display(Prompt = "Cell Number" )]
        public string CellNumber { get; set; }

        [Required]
        [Display(Prompt = "Account Number" )]
        public string AccountNumber { get; set; }

        [Display(Prompt = "Password" )]
        public string Password { get; set; } 
        //Using this as a honeypot
        public string RepeatPassword { get; set; }
    }
}