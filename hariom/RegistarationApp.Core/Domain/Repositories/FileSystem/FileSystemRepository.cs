using Newtonsoft.Json;
using RegistarationApp.Core.Domain.Entities;
using RegistarationApp.Core.Models.Setting;
using RegistartionApp.Core.Domain.Repositories;

namespace RegistarationApp.Core.Domain.Repositories.FileSystem
{
    /// <summary>
    /// File System Repository for T type entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileSystemRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Base path to store data in file
        /// </summary>
        private readonly string _basePath;

        public FileSystemRepository(RepositorySettings repositorySettings)
        {
            _basePath = $"{repositorySettings?.FileSystemSettings?.FilePath}{typeof(T).Name}s.json";
        }

        /// <summary>
        /// Get t type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetById(string id)
        {
            var data = await GetAll();
            return data?.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Get all t type records
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>?> GetAll()
        {
            if (!File.Exists(_basePath))
                return new List<T>();

            var json = await File.ReadAllTextAsync(_basePath);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        /// <summary>
        /// Add T type record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Add(T entity)
        {
            List<T> data = (await GetAll())?.ToList() ?? new();
            data?.Add(entity);
            await WriteToFile(data);
        }
        /// <summary>
        ///Update T type record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async Task<T?> Update(T entity)
        {
            List<T> data = (await GetAll())?.ToList() ?? new();
            var existingEntityIndex = data.FindIndex(e => e.Id == entity.Id);
            if (existingEntityIndex != -1)
            {
                data[existingEntityIndex] = entity;
                await WriteToFile(data);
                return entity;
            }
            return null;
        }
        /// <summary>
        /// Delete T type reocrd
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            List<T> data = (await GetAll())?.ToList() ?? new();
            var removed = data.RemoveAll(r => r.Id == id) > 0;
            if (removed)
            {
                await WriteToFile(data);
            }
            return removed;
        }

        /// <summary>
        /// Private function for write data in file
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task WriteToFile(List<T>? entities)
        {
            if (entities != null)
            {
                string directoryPath = Path.GetDirectoryName(_basePath) ?? "";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var json = JsonConvert.SerializeObject(entities, Formatting.Indented);
                await File.WriteAllTextAsync(_basePath, json);
            }
        }
    }
}