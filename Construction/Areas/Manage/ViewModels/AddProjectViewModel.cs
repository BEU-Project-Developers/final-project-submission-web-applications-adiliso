using System.ComponentModel.DataAnnotations;

namespace Construction.Areas.Manage.ViewModels;

public class AddProjectViewModel
{
    [Required] public string Name { get; set; }

    public string Description { get; set; }

    [Display(Name = "Project Date")] public string ProjectDate { get; set; }

    public string Client { get; set; }

    [Display(Name = "Service")] public string SelectedService { get; set; }

    public IFormFile Image { get; set; }
}