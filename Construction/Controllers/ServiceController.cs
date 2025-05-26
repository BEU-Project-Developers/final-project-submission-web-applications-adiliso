using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class ServiceController : Controller
{
    public IActionResult Services()
    {
        return View();
    }
}