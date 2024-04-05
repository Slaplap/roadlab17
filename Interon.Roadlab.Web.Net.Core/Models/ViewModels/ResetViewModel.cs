using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
    public class ResetViewModel
    {
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public int OTP { get; set; }
        
    }
}