using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RegistarationApp.Core.Models.Common;

namespace RegistrationApp.Server.Filters
{
    /// <summary>
    /// Global Model Validation Filter
    /// </summary>
    public class GlobalModelValidationFilter : IActionFilter
    {
        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccessful = false,
                    Message = GetAllModelStateErrors(context.ModelState),
                    Result = Result.Failed,
                    CorrelationId = null
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
        /// <summary>
        /// On Action Executed
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed
        }

        /// <summary>
        /// Get All Model State Errors
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private static string GetAllModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            if (modelState != null && modelState.Count > 0)
            {
                foreach (var modelStateEntry in modelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
            }
            return string.Join(", ", errors);
        }
    }
}
