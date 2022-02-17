using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        [Display(Name = "Email",Prompt = "Email")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password",Prompt = "Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = true;
    }
}