using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
    public class ResetRequestViewModel
    {
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        

    }
}