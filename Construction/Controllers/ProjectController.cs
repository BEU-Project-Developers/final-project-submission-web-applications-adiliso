using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Construction.Controllers;

public class ProjectController : Controller
{
    private readonly AppDbContext _context;

    public ProjectController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Projects(string? service)
    {
        var services = await _context.Services.ToListAsync();
        ViewBag.Services = services;
        var query = _context.Projects
            .Include(p => p.Service)
            .Include(p => p.Photos)
            .AsQueryable();

        if (!string.IsNullOrEmpty(service))
        {
            query = query.Where(p => p.Service.Name == service);
        }

        var projects = await query.ToListAsync();
        return View(projects);
    }


    public async Task<IActionResult> ProjectDetails(long id)
    {
        var query = _context.Projects
            .Include(p => p.Service)
            .Include(p => p.Photos)
            .AsQueryable();
        query = query.Where(p => p.Id == id);
        var project = await query.FirstOrDefaultAsync();
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
}