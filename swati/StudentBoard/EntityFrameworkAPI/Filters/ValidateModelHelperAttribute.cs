using EntityFrameworkAPI.Enums;
using EntityFrameworkAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EntityFrameworkAPI.Filters
{
    public class ValidateModelHelperAttribute : ActionFilterAttribute
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
                    new ResponseMetaData<string>()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsError = true,
                        ErrorDetails = MessageConstants.ValidationFailed,
                        Message = $"[{string.Join(" , ", errors.ToArray())}]",
                        Result = null
                    }
                );
                base.OnActionExecuting(context);
            }
        }
    }
}
