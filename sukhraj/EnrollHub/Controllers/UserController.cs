using EnrollHub.Application;
using EnrollHub.Application.Dtos.Model;
using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnrollHub.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations such as registration, role assignment, login, and logout.
    /// </summary>
    [Route("user")]
    [Authorize(Roles = Constants.Admin)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        public UserController(IUserService userService, ICommonService commonService)
        {
            _userService = userService;
            _commonService = commonService;
        }

        /// <summary>
        /// Displays the registration form for a new user.
        /// </summary>
        [HttpGet("register")]
        public IActionResult RegisterUser()
        {
            return View();
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddUser(model);
                return View();
            }
            else
            {
                return View();
            }

        }

        /// <summary>
        /// Displays the form for assigning roles to users.
        /// </summary>
        [HttpGet("assign-role")]
        public async Task<IActionResult> AssignRole()
        {
            ViewBag.Users = new SelectList(await _commonService.GetAllActiveUsers(), "Key", "Value");
            ViewBag.Roles = new SelectList(await _commonService.GetAllActiveRoles(), "Value", "Value");
            return View();
        }
        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRole assignRole)
        {
            ViewBag.Users = new SelectList(await _commonService.GetAllActiveUsers(), "Key", "Value");
            ViewBag.Roles = new SelectList(await _commonService.GetAllActiveRoles(), "Value", "Value");

            if (ModelState.IsValid)
            {
                await _userService.AssignRole(assignRole);
                return View();
            }
            else
            {
                return View();
            }

        }

        /// <summary>
        /// Displays the login form.
        /// </summary>
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login()

        {
            return View();
        }

        /// <summary>
        /// Logs in the user.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //As I have used SignInManager Class object to login, if user is login successfull, It will load its all claims by default,
                //no need explicitly to get them and load like we are doing in WEB API to load claims in Token
                var response = await _userService.Login(model);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index", "Enroll");
                }
                else
                {
                    ViewBag.InvalidCredentils = "Invalid Credentials.";
                }
            }
            return View();
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOut();
            return await Task.FromResult(RedirectToAction("Login", "User"));
        }

    }
}
