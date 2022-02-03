using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class PasswordChangeViewModel 
    {
        [Required]
        public string Password { get; set; }
    }
}