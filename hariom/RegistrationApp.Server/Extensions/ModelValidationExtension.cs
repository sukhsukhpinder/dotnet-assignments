using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Server.Filters;

namespace RegistarationApp.Server.Extensions
{
    /// <summary>
    /// Model Validation Extension
    /// </summary>
    public static class ModelValidationExtension
    {
        /// <summary>
        /// Extention for Use Model Validation Handler
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseModelValidationHandler(this IServiceCollection services)
        {
            //Disable default model validation
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            //Global Model Validation Filter
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalModelValidationFilter));
            });

            return services;
        }
    }
}
