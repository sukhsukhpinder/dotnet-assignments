using Microsoft.EntityFrameworkCore;
using RegistarationApp.Core.Domain.Entities;
using RegistarationApp.Core.Domain.Repositories.FileSystem;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.Setting;
using RegistartionApp.Core.Domain.Repositories;

namespace RegistarationApp.Core.Domain.Repositories
{
    public static class RepositoryFactory
    {
        /// <summary>
        /// Create repository instance as per the requirement for T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repositorySettings"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IRepository<T> CreateRepository<T>(RepositorySettings? repositorySettings, DbContext dbContext) where T : BaseEntity
        {
            if (repositorySettings == null)
                throw new ArgumentException(Message.InvalidRepositorySetting);
            if (string.IsNullOrEmpty(repositorySettings?.RepositoryType))
                throw new ArgumentException(Message.InvalidFilePath);

            // Define a dictionary to map repository types to their implementations
            var repositoryMap = new Dictionary<string, Func<IRepository<T>>>
            {
                [StorageType.Fileystem.ToString()] = () =>
                {
                    if (string.IsNullOrEmpty(repositorySettings?.FileSystemSettings?.FilePath))
                        throw new ArgumentException(Message.InvalidFilePath);
                    return new FileSystemRepository<T>(repositorySettings);
                },
                [StorageType.Database.ToString()] = () =>
                {
                    // Define another dictionary to map data access layers to their repository implementations
                    var dataAccessLayerMap = new Dictionary<string, Func<IRepository<T>>>
                    {
                        [DataAccessLayer.EntityFrameworkCore.ToString()] = () => new Database.EntityFrameworkCore.DatabaseRepository<T>(dbContext),
                        [DataAccessLayer.Dapper.ToString()] = () => new Database.Dapper.DatabaseRepository<T>(repositorySettings),
                        [DataAccessLayer.ADO.ToString()] = () => new Database.ADO.DatabaseRepository<T>(repositorySettings)
                    };

                    if (!dataAccessLayerMap.TryGetValue(repositorySettings?.DatabaseSettings?.DataAccessLayer ?? DataAccessLayer.EntityFrameworkCore.ToString(), out var repositoryFactory))
                        throw new ArgumentException(Message.InvalidDataAccesslayer);

                    return repositoryFactory();
                }
            };

            if (!repositoryMap.TryGetValue(repositorySettings.RepositoryType, out var factory))
                throw new ArgumentException(Message.InvalidStorageType);

            return factory();
        }
    }
}
