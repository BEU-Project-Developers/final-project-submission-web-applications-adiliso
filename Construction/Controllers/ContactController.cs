using Construction.Models;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["ActivePage"] = "Contact";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                TempData["Message"] = "Your message has been sent successfully.";
                return RedirectToAction("Contact");
            }

            ViewData["ActivePage"] = "Contact";
            return View(contact);
        }
    }
}