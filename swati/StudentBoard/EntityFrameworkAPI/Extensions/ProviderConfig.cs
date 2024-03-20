using AuthenticationManager.Data;
using EntityFramework.EntityFrameworkAPI.Infrastructure.Data;
using EntityFrameworkAPI.Core.Repository;
using EntityFrameworkAPI.Core.UnitOfWork;
using EntityFrameworkAPI.Infrastructure.Repository;
using EntityFrameworkAPI.Infrastructure.UnitOfWork;
using EntityFrameworkAPI.Services.Services;
using EntityFrameworkAPI.Services.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkAPI.Extensions
{
    public static class ProviderConfig
    {

        public static void AddProviders(this IServiceCollection services)
        {
            var connString = ConfigSettings.ConnectionString;

            services.AddDbContext<EFDbContext>(o => o.UseSqlServer(connString));

            services.AddScoped<IUnit, Unit>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IXMLUnit, XMLUnit>();
            services.AddScoped<IXMLStudentRepository, XMLStudentRepository>();

            services.AddScoped<IDapperUnit, DapperUnit>();
            services.AddScoped<IDapperStudentRepository, DapperStudentRepository>();





        }

    }
}
