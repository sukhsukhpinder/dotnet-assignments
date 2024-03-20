using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SchoolWebAppAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebAppAPI.Helpers
{

    public class GlobalModelValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalModelValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!context.Request.ContentType?.ToLower().Contains("application/json") == true)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid content type. JSON expected.");
                return;
            }

            var modelState = await ValidateJsonModelAsync(context.Request.Body);

            if (!modelState.IsValid)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = modelState
                    .Where(entry => entry.Value.Errors.Any())
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value.Errors.Select(error => error.ErrorMessage).ToArray()
                    );

                var result = new BadRequestObjectResult(errors);
                await result.ExecuteResultAsync(new ActionContext(context, new RouteData(), new ControllerActionDescriptor()));
                return;
            }

            await _next(context);
        }



        private async Task<ModelStateDictionary> ValidateJsonModelAsync(Stream requestBody)
        {
            using var reader = new StreamReader(requestBody);
            var json = await reader.ReadToEndAsync();

            // Deserialize JSON to your model
            var model = JsonConvert.DeserializeObject<Registration>(json);

            // Perform validation on your model
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Populate ModelStateDictionary with validation results
            var modelState = new ModelStateDictionary();
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    modelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }

            return modelState;
        }
    }

}
