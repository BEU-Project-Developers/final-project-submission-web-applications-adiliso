using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Construction.Controllers;

public class ServiceController : Controller
{
    private readonly AppDbContext _context;

    public ServiceController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Services()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }
}