using Construction.Models;
using Construction.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Construction.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = HttpContext.Session.GetString("AdminUsername");
            if (!string.IsNullOrEmpty(username))
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                ViewBag.CurrentAdmin = user;
            }
            base.OnActionExecuting(context);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Verify the password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Store the username in session (for demonstration purposes)
            // Consider using ASP.NET Core Identity SignInManager or JWT in production
            HttpContext.Session.SetString("AdminUsername", user.Username);

            // Optional: set success message
            TempData["Success"] = "Logged in successfully!";

            return RedirectToAction("Index", "Dashboard", new { area = "Manage" });
        }   
    }
}