namespace Construction.Areas.Manage.ViewModels
{
    public class EditServiceViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    
        public string? ExistingLogo { get; set; }

        public IFormFile? Logo { get; set; } // <-- no [Required] here!
    }
}

