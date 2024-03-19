using Microsoft.EntityFrameworkCore;
using RegistarationApp.Core.Domain.Entities;
using RegistartionApp.Core.Domain.Repositories;

namespace RegistarationApp.Core.Domain.Repositories.Database.EntityFrameworkCore
{
    /// <summary>
    /// Database Repository for the EF Core DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _dbContext;

        public DatabaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <summary>
        /// Get Record for T Type Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetById(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Get All Record for T Type Entity
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>?> GetAll()
        {
            return await Task.FromResult(_dbContext.Set<T>().AsEnumerable());
        }

        /// <summary>
        /// Add T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Update T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T?> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Delete T Type Entity record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var entity = await GetById(id);
            if (entity == null)
                return false;

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
