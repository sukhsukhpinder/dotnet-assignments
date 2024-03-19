using EnrollHub.Domain.Entities;
using EnrollHub.Infrastructure;
using EnrollHub.Infrastructure.ServiceExtensions;
using EnrollHub.Application.ServiceExtensions;
using EnrollHub.Middlewares;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace EnrollHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();


            // Add services to the container.

            builder.Host.UseSerilog();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbConnections(configuration);
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<EnrollDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(
                options =>
                {
                    options.AccessDeniedPath = new PathString("/User/AccessDenied");
                    options.LoginPath = new PathString("/User/Login");
                });

            builder.Services.AddServices();
            builder.Services.AddRepositories(configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}