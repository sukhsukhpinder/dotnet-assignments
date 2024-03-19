using Microsoft.AspNetCore.Identity;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Infrastructure;

namespace sf_dotenet_mod.Configuration
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EnrollDbContext>()
            .AddDefaultTokenProviders();
        }
    }

}
