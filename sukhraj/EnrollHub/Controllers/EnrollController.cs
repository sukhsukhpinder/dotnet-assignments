using EnrollHub.Application;
using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Services.Base;
using EnrollHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace EnrollHub.Controllers
{
    /// <summary>
    /// Controller for managing enrollment-related operations such as student registration, enrollment updates, and deletion.
    /// </summary>
    [Route("enroll")]
    [Authorize(Roles = Constants.Admin)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EnrollController : Controller
    {
        private readonly ILogger<EnrollController> _logger;
        private readonly ICommonService _commonService;
        private readonly IStudentService _studentService;

        public EnrollController(ILogger<EnrollController> logger, ICommonService commonService, IStudentService studentService)
        {
            _logger = logger;
            _commonService = commonService;
            _studentService = studentService;

        }

        /// <summary>
        /// Displays the main enrollment page.
        /// </summary>
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Displays the registration form for enrolling a new student.
        /// </summary>
        [HttpGet("register")]
        public async Task<IActionResult> Enroll()
        {
            ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value");
            ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value");
            return View();
        }
        /// <summary>
        /// Registers a new student.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Enroll(StudentRequest request)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user's identity
                var identity = User.Identity as ClaimsIdentity;
                var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                await _studentService.EnrollStudent(request, userIdClaim?.Value);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value");
                ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value");
                return View(request);
            }

        }
        /// <summary>
        /// Displays the form for updating student enrollment.
        /// </summary>
        [HttpGet("{studentId}/enrollment")]
        public async Task<IActionResult> UpdateEnrollment(string studentId)
        {
            if (Guid.TryParse(studentId, out Guid id))
            {
                var response = await _studentService.GetById(studentId);

                ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value", response.Result.StateId);
                ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value", response.Result.CourseId);
                return View(response.Result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Updates student enrollment.
        /// </summary>
        [HttpPost("{studentId}/enrollment")]
        public async Task<IActionResult> UpdateEnrollment(StudentRequest request, string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentNullException(nameof(studentId), Constants.StudentIdCannotBeNull);
            }

            if (ModelState.IsValid)
            {
                // Retrieve the user's identity
                var identity = User.Identity as ClaimsIdentity;
                string userId = identity?.FindFirst(ClaimTypes.NameIdentifier).Value;

                var response = await _studentService.UpdateEnrollment(request, studentId, userId);

                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index", "Enroll");
                }
                else
                {
                    ViewBag.UpdateFailed = Constants.OperationFailed;
                    return View(request);
                }
            }
            else
            {
                ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value");
                ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value");
                return View(request);
            }
        }
        /// <summary>
        /// Retrieves all enrollments.
        /// </summary>
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var response = await _studentService.GetAll();

            var result = JsonConvert.SerializeObject(new { data = response.Result });
            return Ok(result);
        }
        /// <summary>
        /// Deletes a student's enrollment.
        /// </summary>
        [HttpPost("delete/enrollment")]
        public async Task<IActionResult> DeleteEnrollment([FromForm] string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                {
                    throw new ArgumentNullException(nameof(studentId), Constants.StudentIdCannotBeNull);
                }

                var response = await _studentService.DeleteEnrollmentById(studentId);

                return Ok(response.IsSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Ok(false);
            }
        }

        /// <summary>
        /// Privacy policy page.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Error handling.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}