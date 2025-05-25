using System.ComponentModel.DataAnnotations;

namespace Construction.Models;

public class Project
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public List<Photo> Photos { get; set; }
    public DateOnly ProjectDate { get; set; }
    public string Client { get; set; }
    [Required]
    public long ServiceId { get; set; }
    public Service Service { get; set; }
}