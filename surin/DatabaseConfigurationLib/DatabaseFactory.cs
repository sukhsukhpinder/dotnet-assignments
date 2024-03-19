using Database;
using Database.Contracts;
using Database.Implementations.FileRepository;
using Database.Implementations.SqlRepository;
using Database.Implementations.SqlRepository.Common;
using Database.Implementations.SqlRepository.Students;
using Database.Implementations.SqlRepository.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseConfigurationLib
{
    public static class DatabaseFactory
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new DatabaseConfigurationOptions();

            options.DatabaseType = configuration["DatabaseConfiguration:DatabaseType"]!;
            options.DataAccessTechnology = configuration["DatabaseConfiguration:DataAccessTechnology"]!;

            switch (options.DatabaseType.ToLower())
            {
                case "filestudentdb":
                    ConfigureFileDatabase(services, configuration);
                    break;

                default:
                    ConfigureSqlDatabase(services, configuration, options);
                    break;
            }

            return services;
        }

        private static void ConfigureSqlDatabase(IServiceCollection services, IConfiguration configuration, DatabaseConfigurationOptions options)
        {
            services.AddDbContext<RegistrationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Registration.API")));

            services.AddScoped<IUserContract, UserDatacontext>();
            services.AddScoped<IStudentContract, StudentDataContext>();
            services.AddScoped<ICommonContract, CommonDataContext>();

            ConfigureDataAccessTechnology(services, configuration, options);

        }

        private static void ConfigureFileDatabase(IServiceCollection services, IConfiguration configuration)
        {
            // registration for JwtTokenHandler
            services.AddDbContext<RegistrationDBContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly("Registration.API")));

            services.AddScoped<IUserContract, FileUserRepository>();
            services.AddScoped<IStudentContract, FileStudentRepository>();
            services.AddScoped<ICommonContract, FileCommonRepository>();
        }

        private static void ConfigureDataAccessTechnology(IServiceCollection services, IConfiguration configuration, DatabaseConfigurationOptions options)
        {
            switch (options.DataAccessTechnology.ToLower())
            {
                case "efcore":
                    ConfigureEfCore(services, configuration);
                    break;

                case "dapper":
                    ConfigureDapper(services, configuration);
                    break;

                case "ado":
                    ConfigureAdoNet(services, configuration);
                    break;

                default:
                    ConfigureEfCore(services, configuration);
                    break;
            } 

        }

        private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RegistrationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Registration.API")));

            services.AddScoped<IUserContract, UserDatacontext>();
            services.AddScoped<IStudentContract, StudentDataContext>();
            services.AddScoped<ICommonContract, CommonDataContext>();
        }

        private static void ConfigureAdoNet(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserContract>(provider => new UserAdoRepository(configuration.GetConnectionString("DefaultConnection")!));
            services.AddScoped<ICommonContract>(provider => new CommonAdoRepository(configuration.GetConnectionString("DefaultConnection")!));
            services.AddScoped<IStudentContract>(provider => new StudentAdoRepository(configuration.GetConnectionString("DefaultConnection")!));
        }

        private static void ConfigureDapper(IServiceCollection services, IConfiguration configuration)
        {      
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var sqlConnection = new SqlConnection(connectionString);

            services.AddScoped<IStudentContract>(provider => new StudentDapperRepository(sqlConnection));
            services.AddScoped<IUserContract>(provider => new UserDapperRepository(sqlConnection));
            services.AddScoped<ICommonContract>(provider => new CommonDapperRepository(sqlConnection));
        }

    }
}
