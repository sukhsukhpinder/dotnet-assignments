using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RegistarationApp.Core.Models.Setting;
using System.Text;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Jwt Authentication Extention to configure Jwt auth
    /// </summary>
    public static class JwtAuthenticationExtention
    {
        /// <summary>
        /// Extention menthod for the JWT Authentication
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseJwtAuthenticationHandler(this IServiceCollection services)
        {
            //get Jwt setting
            var jwtSetting = services.BuildServiceProvider().GetService<JwtSetting>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting?.Key ?? string.Empty)),
                        ValidateIssuer = false,
                        //ValidIssuer= jwtSetting.Issuer,
                        ValidateAudience = false,
                        //ValidAudience= jwtSetting.Audience
                    };
                });
            return services;
        }
    }
}
