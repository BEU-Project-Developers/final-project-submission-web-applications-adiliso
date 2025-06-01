using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class ProjectController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Create()
    {
        return View();
    }
}