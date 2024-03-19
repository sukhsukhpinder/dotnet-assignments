using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RegistarationApp.Core.Models.Common;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Api Controller Extension for common response model 
    /// </summary>
    public static class ApiControllerExtension
    {
        /// <summary>
        /// Response for api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="statusCode"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <param name="correlationId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IActionResult Response<T>(this ControllerBase controller, int statusCode, bool isSuccessful, string? message = null, T? result = null, string? correlationId = null, ModelStateDictionary? modelState = null) where T : class
        {
            ApiResponse<T> response = new()
            {
                StatusCode = statusCode,
                IsSuccessful = isSuccessful,
                Message = modelState != null && modelState.Count > 0 ? GetAllModelStateErrors(modelState) : message,
                Result = result,
                CorrelationId = correlationId
            };

            return new JsonResult(response)
            {
                StatusCode = statusCode
            };
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
