using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Domain.Entities;

namespace sf_dotenet_mod.Controllers
{
    /// <summary>
    /// Controller for handling user account operations such as login and logout.
    /// </summary>
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Constructor for the AccountController.
        /// </summary>
        public AccountController(SignInManager<Users> signInManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request for user login.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "student");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        [HttpGet("logout")]
        [Authorize] // Ensures that only authenticated users can access this action
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Signs out the current user
            return RedirectToAction("login", "account"); // Redirects to the login page after logout
        }

    }
}
