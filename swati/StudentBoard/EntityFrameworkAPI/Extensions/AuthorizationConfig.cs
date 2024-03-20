using Microsoft.AspNetCore.Authorization;

namespace EntityFrameworkAPI.Extensions
{
    public static class AuthorizationConfig
    {
        public static void AddJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Bearer")
                    .RequireClaim("Role")
                    .Build();
            });
        }
    }
}
