using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class PasswordChangeViewModel 
    {
        [Required]
        public string Password { get; set; }
    }
}