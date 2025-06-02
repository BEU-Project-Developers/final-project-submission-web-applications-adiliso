using Construction.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Areas.Manage.Controllers;

[Area("Manage")]
public class PageController : Controller
{
    public IActionResult Tables()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Image != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }
            
            return RedirectToAction("Index");
        }

        return View(model);
    }
}