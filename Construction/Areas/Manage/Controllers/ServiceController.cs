using Construction.Areas.Manage.ViewModels;
using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class ServiceController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ServiceController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddService(AddServiceViewModel model)
    {
        if (ModelState.IsValid)
        {
            var service = new Service()
            {
                Name = model.Name,
                Description = model.Description
            };

            if (model.Logo != null && model.Logo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/services");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Logo.CopyToAsync(fileStream);
                }

                service.Logo = uniqueFileName;
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Service added successfully!";
            return RedirectToAction("Create");
        }

        return RedirectToAction("Create");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }

        var model = new EditServiceViewModel
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            ExistingLogo = service.Logo
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditServiceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var service = await _context.Services.FindAsync(model.Id);
        if (service == null)
        {
            return NotFound();
        }

        service.Name = model.Name;
        service.Description = model.Description;

        if (model.Logo != null && model.Logo.Length > 0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/services");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Logo.CopyToAsync(fileStream);
            }

            // Delete old logo if needed
            if (!string.IsNullOrEmpty(service.Logo))
            {
                string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, service.Logo.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            service.Logo = uniqueFileName;
        }

        _context.Services.Update(service);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Service updated successfully!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(long id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Service deleted successfully!";
        return RedirectToAction("Index");
    }
}