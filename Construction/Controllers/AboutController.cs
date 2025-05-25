using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class AboutController : Controller
{
    public IActionResult About()
    {
        return View();
    }
}