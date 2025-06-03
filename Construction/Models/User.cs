using System.ComponentModel.DataAnnotations;

namespace Construction.Models;

public class User
{
    public long Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    
    public string? ProfilePhoto { get; set; }
}