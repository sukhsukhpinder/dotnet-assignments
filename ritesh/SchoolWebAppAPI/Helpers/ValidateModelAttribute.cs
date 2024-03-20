using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolWebAppAPI.Models;
using System.Net;

namespace SchoolWebAppAPI.Helpers
{
    /// <summary>
    /// ValidateModelAttribute
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// ValidateModel action Attribute for all common form submits
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IList<string>errorlist=new List<string>();
                var errors = context.ModelState.AsEnumerable();
                foreach(var error in errors)
                {
                    foreach(var inner in error.Value!.Errors)
                    {
                        errorlist.Add(inner.ErrorMessage);
                    }
                }
                var res= (new ResponseModel<IList<string>>
                {
                    IsSuccessful = false,
                    Message = ResponseMessages.ValidationError,
                    Data= errorlist,
                    StatusCode= StatusCodes.Status400BadRequest
                });
                context.Result = new BadRequestObjectResult(res);
            }
            base.OnActionExecuting(context);
        }
    }

}