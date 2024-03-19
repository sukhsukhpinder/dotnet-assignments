using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseConfigurationLib.Common
{
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new();
                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new ObjectResult(
                    new ServiceResponse<string>()
                    {
                        Status = System.Net.HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        ErrorDetails = $"[{string.Join(" , ", errors.ToArray())}]",
                        Result = null
                    }
                );
                base.OnActionExecuting(context);
                
            }
        }      
    }
}
