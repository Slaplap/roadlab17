using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI.HtmlControls;

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

    public HtmlInputFile Files { get; set; }
    //honeypot
    public string Surname { get; set; }
}