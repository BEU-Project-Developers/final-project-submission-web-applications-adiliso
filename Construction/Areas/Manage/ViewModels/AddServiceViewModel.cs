using System.ComponentModel.DataAnnotations;

namespace Construction.Areas.Manage.ViewModels;

public class AddServiceViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public IFormFile Logo { get; set; }
}