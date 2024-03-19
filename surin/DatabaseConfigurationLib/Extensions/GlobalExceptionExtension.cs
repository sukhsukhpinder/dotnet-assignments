using DatabaseConfigurationLib.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DatabaseConfigurationLib.Extensions
{
    public static class GlobalExceptionExtension
    {
        /// <summary>
        /// Extension method to handle exceptions globally
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            #region Global Exception Middleware

            // Global middleware which will be invoked one any exception is triggered within the application
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            /* 
             * Global middleware which will be invoked one any exception is triggered or custom exception is thrown within the application
             * Use if application have requirement for custom exception 
             */
            //app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            #endregion
            return app;
        }

       
    }
}
