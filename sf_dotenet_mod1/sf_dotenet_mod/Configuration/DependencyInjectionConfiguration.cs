using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sf_dotenet_mod.Application.Mappers;
using sf_dotenet_mod.Application.Services;
using sf_dotenet_mod.Application.Services.Base;
using sf_dotenet_mod.Domain.Repositories;
using sf_dotenet_mod.Infrastructure;
using sf_dotenet_mod.Infrastructure.Repositories;

namespace sf_dotenet_mod.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencies(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            #region Register repositories
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IDataSourceProvider, DataSourceProvider>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<ICommonRepository, FileCommonRepository>();
            services.AddScoped<IStudentRepository, FileStudentRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICommonRepository, AdoCommonRepository>();
            services.AddScoped<IStudentRepository, AdoStudentRepository>();
            services.AddScoped<SqlConnectionFactory>();
            #endregion

            // Call the method to configure AutoMapper
            ConfigureAutoMapper(services);
            // Call the method to configure database
            ConfigureDatabase(services, configuration);
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(Program));

            // Configure AutoMapper profiles
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            //// Set the current data source
            //services.Configure<DataSourceProvider>(options =>
            //{
            //    options.CurrentDataSource = DataSource.ado; // Set your desired data source here for ado
            //});

            //services.AddDbContext<AppDbContext>((sp, o) =>
            //    o.UseSqlServer(sp.GetRequiredService<IDataSourceProvider>().GetConnectionString()));

            services.AddDbContext<EnrollDbContext>(op =>
            op.UseSqlServer(configuration.GetConnectionString("sf_dotenet_modConnection"),
                b => b.MigrationsAssembly("sf_dotenet_mod")),
                ServiceLifetime.Singleton);

            services.AddDbContext<PrimaryDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Primary")));
            services.AddDbContext<SecondaryDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Secondary")));


            //var serviceProvider = services.BuildServiceProvider();
            //var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        }
    }
}
