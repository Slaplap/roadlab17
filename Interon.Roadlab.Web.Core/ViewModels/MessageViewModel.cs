using System;
using System.ComponentModel.DataAnnotations;

namespace Interon.Roadlab.Web.Core.ViewModels
{
    public class MessageViewModel
    {
        [Required]
        public Guid Key { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public bool? Accept { get; set; }
    }
}