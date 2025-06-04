using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class AboutController : Controller
{
    private readonly AppDbContext _context;

    public AboutController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult About()
    {
        var teamMembers = _context.TeamMembers.ToList();
        return View(teamMembers);
    }
}