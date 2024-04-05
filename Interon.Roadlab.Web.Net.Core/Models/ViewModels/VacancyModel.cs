using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
	public class VacancyModel
	{
		[Required]
		[Display(Name = "Name", Prompt = "Name")]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email", Prompt = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Contact Number")]
		public string ContactNumber { get; set; }

		public IFormFile Files { get; set; }

		//honeypot
		public string Surname { get; set; }
	}
}