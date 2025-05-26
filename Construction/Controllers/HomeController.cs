using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Blog() => View();
    public IActionResult BlogDetails() => View();
}