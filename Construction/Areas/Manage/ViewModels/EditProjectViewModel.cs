using Construction.Models;

namespace Construction.Areas.Manage.ViewModels
{
    public class EditProjectViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long? ServiceId { get; set; }

        public List<IFormFile>? NewPhotos { get; set; }

        public List<Photo>? ExistingPhotos { get; set; }
    }
}
