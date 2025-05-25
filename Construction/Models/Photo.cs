using System.ComponentModel.DataAnnotations;

namespace Construction.Models;

public class Photo
{
    public long Id { get; set; }
    [Required]
    public string PhotoPath { get; set; }
    [Required]
    public long ProjectId { get; set; }
    public Project Project { get; set; }
}