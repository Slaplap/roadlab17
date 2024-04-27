using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
    public class PasswordChangeViewModel 
    {
        [Required]
        public string Password { get; set; }
    }
}