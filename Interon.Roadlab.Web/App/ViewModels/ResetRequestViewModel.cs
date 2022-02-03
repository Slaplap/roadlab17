using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.App.ViewModels
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