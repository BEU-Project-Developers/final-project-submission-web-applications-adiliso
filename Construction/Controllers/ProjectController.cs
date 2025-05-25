using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class ProjectController : Controller
{
    public IActionResult Projects()
    {
        return View();
    }
    public IActionResult ProjectDetails()
    {
        return View();
    }
}