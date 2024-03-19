using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sf_dotenet_mod.Infrastructure
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IEnumerable<IRepository> _repositories;

        public RepositoryFactory(IEnumerable<IRepository> repositories)
        {
            _repositories = repositories;
        }

        public IRepository GetRepository(DataSource dataSource)
        {
            return dataSource switch
            {
                DataSource.Primary => _repositories.Single(r => r is PrimaryRepository),
                DataSource.Secondary => _repositories.Single(r => r is SecondaryRepository),
                _ => throw new ArgumentOutOfRangeException(nameof(dataSource), dataSource, null)
            };
        }
    }

    public interface IRepositoryFactory
    {
        IRepository GetRepository(DataSource dataSource);
    }
}
