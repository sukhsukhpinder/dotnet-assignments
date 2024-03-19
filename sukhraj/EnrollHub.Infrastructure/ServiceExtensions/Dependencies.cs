using EnrollHub.Domain.Repositories;
using EnrollHub.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace EnrollHub.Infrastructure.ServiceExtensions
{
    public static class Dependencies
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUsersRepository, UserRepository>();

            switch (configuration["DataBaseType"]?.ToLower())
            {
                case Constants.DataBaseTypeFile:
                    services.AddScoped<IStudentRepository, FileStudentRepository>();
                    services.AddScoped<ICommonRepository, FileCommonRepository>();
                    break;
                case Constants.DataBaseTypeRdbms:
                    services.AddScoped<IStudentRepository, StudentRepository>();
                    services.AddScoped<ICommonRepository, CommonRepository>();
                    break;
                default:
                    throw new InvalidOperationException(Constants.InvalidDatabaseTypeConfiguration);
            }

            return services;
        }
        public static IServiceCollection AddRepositoriesBasedOnDatabaseType(this IServiceCollection services, )
        {
           
            return services;
        }

        public static IServiceCollection AddDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EnrollDbContext>(op =>
            op.UseSqlServer(configuration.GetConnectionString("EnrollHubConnection"),
            b => b.MigrationsAssembly("EnrollHub")));

            services.AddScoped(provider => new SqlConnection(configuration.GetConnectionString("EnrollHubConnection")));

            // Register Dapper connection
            services.AddScoped<IDbConnection>(provider => new SqlConnection(configuration.GetConnectionString("EnrollHubConnection")));
            //above both connection can be used in single line but here its used just for learning purpose and for demo only.

            //For MVC application when we use AddIdentity, it adds default Authentication Mechanism that is Cookie based, no need to
            //mention exlicitly.But in API as API is stateless and AddIdentity is used even then we need to use Authentication Mechanism
            //like JWT token and need to manage Claims, but in MVC when we are usingSignInManager it get claims and use cookie to hold them
            //by default

            return services;
        }
    }
}
