using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class LoginController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}