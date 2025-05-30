using System.ComponentModel.DataAnnotations;

namespace Construction.Models;

public class Service
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Logo { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}