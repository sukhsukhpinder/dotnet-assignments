using SchoolWebAppAPI.Middlewares;

namespace SchoolWebAppAPI.Helpers
{
    public static class CustomExceptionFilter 
    {
        /// <summary>
        /// For using ConfigureCustomExceptionMiddleware
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
     }
}