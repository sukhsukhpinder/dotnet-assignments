using EntityFrameworkAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace EntityFrameworkAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            this._requestDelegate = requestDelegate;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                ResponseMetaData<string> responseMetadata = new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    IsError = true,
                    ErrorDetails = ex.Message
                };

                var serializedResponseMetadata = JsonConvert.SerializeObject(responseMetadata);
                _logger.LogError(ex, "Exception occurred: {Message}", JsonConvert.SerializeObject(serializedResponseMetadata));

                context.Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(serializedResponseMetadata);
            }
        }
    }
}
