using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace sf_dotenet_mod.Controllers
{
    public class ErrorController(ILogger<ErrorController> logger) : Controller
    {
        private readonly ILogger<ErrorController> _logger = logger;

        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you are request could not be found.";
                    ViewBag.Path = statusCodeResult!.OriginalPath;
                    break;
            }

            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionDetails!.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.ExceptionStackTrace = exceptionDetails.Error.StackTrace;

            _logger.LogError(exceptionDetails.Error, "An error occurred in {Path}", exceptionDetails.Path);

            return View("Error");
        }
    }
}
