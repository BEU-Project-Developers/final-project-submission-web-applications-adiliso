using Construction.Exceptions;
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
        try
        {
            var project = await _context.Projects
                .Include(p => p.Service)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new ProjectNotFoundException($"Project with ID {id} was not found.");
            }

            return View(project);
        }
        catch (ProjectNotFoundException)
        {
            return NotFound();
        }
    }
}