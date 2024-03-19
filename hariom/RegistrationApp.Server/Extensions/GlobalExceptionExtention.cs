using RegistrationApp.Server.Middleware;

namespace RegistarationApp.Server.Extensions
{
    /// <summary>
    /// Global Exception Extention
    /// </summary>
    public static class GlobalExceptionExtention
    {
        /// <summary>
        /// Extention for the Global Exception Extention
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            //Global Exception handler
            app.UseMiddleware<GlobalExceptionMiddleware>();

            return app;
        }
    }
}
