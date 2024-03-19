using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistarationApp.Core.Domain.Repositories;
using RegistarationApp.Core.Models.Setting;
using RegistarationApp.Core.Services;
using RegistartionApp.Core.Domain.Entities;
using RegistrationApp.Core.Domain.Repositories.Database.EntityFrameworkCore;
using RegistrationApp.Core.Services;
using RegistrationApp.Core.Services.Interface;

namespace RegistarationApp.Core.Extentions
{
    public static class RepositoryExtention
    {
        /// <summary>
        /// Extention method for handle the Repositories
        /// </summary>
        /// <param name="services"></param>
        /// <param name="repositorySettings"></param>
        /// <returns></returns>
        public static IServiceCollection UseRepositoryHandler(this IServiceCollection services)
        {
            //get Repository setting
            var repositorySettings = services.BuildServiceProvider().GetService<RepositorySettings>();

            //DBContext EFCore
            services.AddDbContext<RegistrationDBContext>(options => options.UseSqlServer(repositorySettings?.DatabaseSettings?.ConnectionString));

            //Main Repository
            services.AddScoped(provider => RepositoryFactory.CreateRepository<User>(repositorySettings, provider.GetRequiredService<RegistrationDBContext>()));
            services.AddScoped(provider => RepositoryFactory.CreateRepository<Course>(repositorySettings, provider.GetRequiredService<RegistrationDBContext>()));
            services.AddScoped(provider => RepositoryFactory.CreateRepository<Registration>(repositorySettings, provider.GetRequiredService<RegistrationDBContext>()));

            //Services            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();

            return services;
        }
    }
}
