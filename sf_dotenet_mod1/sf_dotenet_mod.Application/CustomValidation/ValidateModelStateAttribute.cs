using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace sf_dotenet_mod.Application.CustomValidation
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // this code works when we are working with web api and it returns the json result of validation errros.
            //if (!context.ModelState.IsValid)
            //{
            //    // If ModelState is not valid, return BadRequest with the validation errors
            //    context.Result = new BadRequestObjectResult(context.ModelState);
            //}

            if (!context.ModelState.IsValid)
            {
                var errorMessage = string.Join("<br/>", context.ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage));
                context.HttpContext.Items["ErrorMessage"] = errorMessage;

                // Get the action name from the route values
                var actionName = context.RouteData.Values["action"]?.ToString();

                context.Result = new ViewResult
                {
                    ViewName = actionName,
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                };
            }
        }
    }
}
