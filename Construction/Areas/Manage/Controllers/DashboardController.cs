using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}