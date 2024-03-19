using Asp.Versioning;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Versioning Extension for configure versioning
    /// </summary>
    public static class VersioningExtension
    {
        /// <summary>
        /// Extention for Version handling
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseVersioningHandler(this IServiceCollection services)
        {
            #region Versioning

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                //new QueryStringApiVersionReader("api-version"),
                //new HeaderApiVersionReader("X-Version"),
                new UrlSegmentApiVersionReader()
                //new MediaTypeApiVersionReader("ver")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            #endregion

            return services;
        }
    }
}
