using Asp.Versioning.ApiExplorer;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Swagger Extension
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Swagger extension method to implement API versioning
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerUIHandler(this IApplicationBuilder app)
        {
            #region Swagger UI Versioning
            app.UseSwaggerUI(options =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider?.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.EnableDeepLinking();
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                options.DefaultModelsExpandDepth(-1);
            });
            #endregion

            return app;
        }
    }
}
