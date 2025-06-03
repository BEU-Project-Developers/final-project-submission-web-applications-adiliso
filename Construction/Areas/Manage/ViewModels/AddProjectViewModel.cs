using System.ComponentModel.DataAnnotations;

namespace Construction.Areas.Manage.ViewModels
{
    public class AddProjectViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public DateOnly ProjectDate { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}