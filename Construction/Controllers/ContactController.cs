using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers;

public class ContactController : Controller
{
    public IActionResult Contact()
    {
        return View();
    }
}