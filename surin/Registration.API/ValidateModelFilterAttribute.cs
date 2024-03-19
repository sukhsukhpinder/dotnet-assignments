using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Registration.API.Dtos;
using System.Net;

namespace Registration.API
{
    public class ValidateModelFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                var serviceResponse = new ServiceResponse<object>
                {
                    Status = HttpStatusCode.BadRequest,
                    IsSuccessful = false,
                    ErrorDetails = JsonConvert.SerializeObject(errors, Formatting.Indented),
                    Result = null
                };

                context.Result = new BadRequestObjectResult(serviceResponse);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
