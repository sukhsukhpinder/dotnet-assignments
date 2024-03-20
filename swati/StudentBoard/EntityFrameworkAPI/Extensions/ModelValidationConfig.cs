using EntityFrameworkAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkAPI.Extensions
{
    public static class ModelValidationConfig
    {
        public static IServiceCollection AddModelValidation(this IServiceCollection services)
        {

            #region Model Validation

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelHelperAttribute));
            });

            #endregion

            return services;
        }
    }
}
