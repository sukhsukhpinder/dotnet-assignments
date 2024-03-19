//using JwtAuthenticationManager.Dto;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System.Net;

//namespace Registration.API
//{
//    public class CustomExceptionHandlerMiddleware
//    {       

//        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
//        private readonly RequestDelegate _next;

//        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception ex)
//            {
//                await HandleException(context, ex);
//            }
//        }
//        public async Task HandleException(HttpContext context, Exception exception)
//        {
//            var code = HttpStatusCode.InternalServerError;

//            var serviceResponse = new ServiceResponse<object>
//            {
//                Status = "Error",
//                IsSuccessful = false,
//                ErrorDetails = exception.Message,
//                Result = null
//            };

//            _logger.LogError(exception, "An error occurred: {ErrorMessage}", exception.Message);

//            var result = JsonConvert.SerializeObject(serviceResponse);
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)code;

//            await context.Response.WriteAsync(result);
//        }
//    }
//}
