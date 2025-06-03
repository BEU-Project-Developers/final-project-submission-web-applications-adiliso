using Construction.Areas.Manage.ViewModels;
using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class ProjectController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var projects = await _context.Projects
            .Include(p => p.Service)
            .ToListAsync();

        return View(projects);
    }

    public async Task<IActionResult> Details(long id)
    {
        var project = await _context.Projects
            .Include(p => p.Service)
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.ServiceList = new SelectList(await _context.Services.ToListAsync(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ServiceList = new SelectList(_context.Services, "Id", "Name");
            return View(model);
        }

        var project = new Project
        {
            Name = model.Name,
            Description = model.Description,
            Client = model.Client,
            ProjectDate = model.ProjectDate,
            ServiceId = model.ServiceId,
            Photos = new List<Photo>()
        };

        if (model.Photos != null && model.Photos.Count > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/projects");
            Directory.CreateDirectory(uploadsFolder);

            foreach (var photo in model.Photos)
            {
                if (photo != null && photo.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                    }

                    project.Photos.Add(new Photo { PhotoPath = uniqueFileName });
                }
            }
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Project created successfully!";
        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var project = await _context.Projects
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        ViewBag.ServiceList = new SelectList(await _context.Services.ToListAsync(), "Id", "Name");

        var model = new EditProjectViewModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ServiceId = project.ServiceId,
            ExistingPhotos = project.Photos?.ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Repopulate dropdowns, etc.
            ViewBag.ServiceList = new SelectList(_context.Services, "Id", "Name");
            return View(model);
        }

        var project = await _context.Projects
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == model.Id);

        if (project == null)
        {
            return NotFound();
        }

        // Apply only non-null updates
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            project.Name = model.Name;
        }

        if (!string.IsNullOrWhiteSpace(model.Description))
        {
            project.Description = model.Description;
        }

        if (model.ServiceId.HasValue)
        {
            project.ServiceId = model.ServiceId.Value;
        }

        if (model.NewPhotos != null && model.NewPhotos.Count > 0)
        {
            foreach (var file in model.NewPhotos)
            {
                // Save the file (handle saving logic here)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img/projects", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                project.Photos.Add(new Photo
                {
                    ProjectId = project.Id,
                    PhotoPath = uniqueFileName
                });
            }
        }

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Project updated successfully!";
        return RedirectToAction("Index");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePhoto(long id)
    {
        var photo = await _context.Photo.FindAsync(id);
        if (photo == null) return NotFound();

        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img/projects", photo.PhotoPath);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Photo.Remove(photo);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Photo deleted successfully!";
        return RedirectToAction("Edit", new { id = photo.ProjectId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var project = await _context.Projects
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var project = await _context.Projects
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        // Delete all related photos from disk and database
        if (project.Photos != null && project.Photos.Any())
        {
            foreach (var photo in project.Photos)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img/projects", photo.PhotoPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Photo.Remove(photo);
            }
        }

        // Remove the project from the database
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Project deleted successfully!";
        return RedirectToAction("Index");
    }
}