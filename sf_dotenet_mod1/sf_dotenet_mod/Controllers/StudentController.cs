using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using sf_dotenet_mod.Application;
using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Application.Services.Base;

namespace sf_dotenet_mod.Controllers
{
    /// <summary>
    /// Controller for managing student operations.
    /// </summary>
    /// <remarks>
    /// Constructor for the StudentController.
    /// </remarks>
    [Route("student")]
    [Authorize(Roles = ConstantKeys.Admin)]
    public class StudentController(ILogger<StudentController> logger, ICommonService commonService, IStudentService studentService) : Controller
    {
        private readonly ILogger<StudentController> _logger = logger;
        private readonly ICommonService _commonService = commonService;
        private readonly IStudentService _studentService = studentService;

        /// <summary>
        /// Displays the index view for the Student controller.
        /// </summary>
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for creating a new student.
        /// </summary>
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value");
            ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value");
            return View();
        }

        /// <summary>
        /// Handles the POST request for creating a new student.
        /// </summary>
        [HttpPost("create")]
        //[ValidateModelState]
        public async Task<IActionResult> Create(StudentRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _studentService.Create(request);
                if (response != null && response.IsSuccessful)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    ViewBag.CreateFailed = ConstantKeys.OperationFailed;
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
        /// Displays the view for editing a student.
        /// </summary>
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.States = new SelectList(await _commonService.GetAllActiveStates(), "Key", "Value");
            ViewBag.Courses = new SelectList(await _commonService.GetAllActiveCourse(), "Key", "Value");
            var result = await _studentService.Get(id);
            return View("Edit", result.Result);
        }

        /// <summary>
        /// Handles the POST request for editing a student.
        /// </summary>
        [HttpPost("edit")]
        //[ValidateModelState]
        public async Task<IActionResult> Edit(StudentRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _studentService.Update(request, request.StudentId);
                if (response != null && response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.UpdateFailed = ConstantKeys.OperationFailed;
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
        /// Gets the details of all students.
        /// </summary>
        [HttpPost("get")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.GetAll();
            return Json(result.Result);
        }

        /// <summary>
        /// Gets the details of a specific student by ID.
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _studentService.Get(id);
            return View("View", result.Result);
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await _studentService.Delete(id);
            return Json(new { success = true, data = true });
        }
        /// <summary>
        /// Displays the privacy view.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
