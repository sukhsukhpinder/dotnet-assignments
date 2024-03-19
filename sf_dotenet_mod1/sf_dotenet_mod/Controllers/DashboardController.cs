using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sf_dotenet_mod.Application.Services.Base;
using System.Text;

namespace sf_dotenet_mod.Controllers
{
    [Route("dashboard")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IStudentService _studentService;

        /// <summary>
        /// Constructor for the DashboardController.
        /// </summary>
        public DashboardController(ILogger<DashboardController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        /// <summary>
        /// Displays the index view for the dashboard controller.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retrieves student chart details.
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var result = await _studentService.GetChartDetails();
            ViewBag.ChartData = result.Result;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var res in result.Result)
            {
                sb.Append("{");
                System.Threading.Thread.Sleep(50);
                string color = String.Format("#{0:X6}", new Random().Next(0x1000000));
                sb.Append(string.Format("text :'{0}', value:{1}, color: '{2}'", res.Key, res.Value, color));
                sb.Append("},");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return Content(sb.ToString());
        }
    }
}
