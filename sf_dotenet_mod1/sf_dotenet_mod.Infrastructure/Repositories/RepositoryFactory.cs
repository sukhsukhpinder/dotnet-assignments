using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _dbType;
        public RepositoryFactory(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _dbType = _configuration["DbType"];
        }

        public ICommonRepository? GetCommonRepository()
        {
            var services = _serviceProvider.GetServices<ICommonRepository>();

            if (_dbType == DataSource.file.ToString())
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(FileCommonRepository));
            }
            else if (_dbType == DataSource.ado.ToString())
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(AdoCommonRepository));
            }
            else
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(CommonRepository));
            }

        }

        public IStudentRepository? GetStudentRepository()
        {
            var services = _serviceProvider.GetServices<IStudentRepository>();

            if (_dbType == DataSource.file.ToString())
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(FileStudentRepository));
            }
            else if (_dbType == DataSource.ado.ToString())
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(AdoStudentRepository));
            }
            else
            {
                return services.FirstOrDefault(s => s.GetType() == typeof(StudentRepository));
            }
        }
    }
}
