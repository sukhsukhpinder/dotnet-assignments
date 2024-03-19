using EnrollHub.Application;
using EnrollHub.Application.Dtos.Model;
using EnrollHub.Application.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnrollHub.Controllers
{
    /// <summary>
    /// Controller for managing home-related operations such as displaying graphs and handling errors.
    /// </summary>
    [Authorize(Roles = Constants.Admin)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonService _commonService;
        public HomeController(ILogger<HomeController> logger, ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Displays the graphs page.
        /// </summary>
        public IActionResult Graphs()
        {
            return View();
        }

        /// <summary>
        /// Retrieves Students State wise data.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStudentStateWise()
        {
            ChartData data = await _commonService.GetStudentStateWise();

            var result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves Student Course Wise.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStudentCourseWise()
        {
            ChartData data = await _commonService.GetStudentCourseWise();

            var result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        /// <summary>
        /// Handles errors and logs exceptions.
        /// </summary>
        public IActionResult Error()
        {
            // Log the exception
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error, Constants.UnexpectedError);
                ViewBag.ExceptionMessage = exceptionFeature.Error;
            }
            else
            {
                _logger.LogError(Constants.UnexpectedErrorNoExceptionInformation);
                ViewBag.ExceptionMessage = Constants.UnexpectedErrorNoExceptionInformation;
            }


            // You can return a specific error view here
            return View();
        }
    }
}
