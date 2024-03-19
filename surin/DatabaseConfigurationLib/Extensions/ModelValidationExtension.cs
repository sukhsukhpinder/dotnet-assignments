using DatabaseConfigurationLib.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseConfigurationLib.Extensions
{
    public static class ModelValidationExtension
    {
        public static IServiceCollection UseModelValidationHandler(this IServiceCollection services)
        {

            #region Model Validation

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilterAttribute));
            });

            #endregion

            return services;
        }
    }
}
