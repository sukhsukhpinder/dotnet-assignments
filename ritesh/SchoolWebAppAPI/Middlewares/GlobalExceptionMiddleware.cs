using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SchoolWebAppAPI.Models;
using System.Net;

namespace SchoolWebAppAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;

        public ExceptionMiddleware(RequestDelegate next, ILog logger)
        {
            _next = next;
            _logger = logger;

        }
        /// <summary>
        ///this method is invoked when exception occured
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
               
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.Error(exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel<string>
            {
                IsSuccessful = false,
                Message = exception.Message,
                StatusCode=ResponseCodes.InternalServerError
            }));
        }
    }
}