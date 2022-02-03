using System;
using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.App.ViewModels
{
    public class ConfirmationViewModel
    {
        [Required]
        public Guid Key { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public bool? Accept { get; set; }
    }
}