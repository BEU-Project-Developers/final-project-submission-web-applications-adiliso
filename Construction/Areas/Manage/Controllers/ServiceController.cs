using Construction.Areas.Manage.ViewModels;
using Construction.Models;
using Microsoft.AspNetCore.Mvc;

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
            return RedirectToAction("Forms", "Page");
        }

        return RedirectToAction("Forms", "Page");
    }
}