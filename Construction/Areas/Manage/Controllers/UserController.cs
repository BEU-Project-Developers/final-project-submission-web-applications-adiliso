using Construction.Areas.Manage.ViewModels;
using Construction.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Construction.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public UserController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _webHostEnvironment = webHostEnvironment;
            
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: Manage/User/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(model);
            }

            string? uniqueFileName = null;

            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "admins");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePhoto.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(fileStream);
                }
            }
            
            var user = new User
            {
                Username = model.Username,
                ProfilePhoto = uniqueFileName
            };

            // Hash the password using PasswordHasher
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "User created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}