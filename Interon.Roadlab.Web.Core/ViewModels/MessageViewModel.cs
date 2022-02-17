using System;
using System.ComponentModel.DataAnnotations;
using Interon.Roadlab.Web.App.Models;

namespace Interon.Roadlab.Web.App.ViewModels
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