namespace RegistartionApp.Core.Domain.Repositories
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Add T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Add(T entity);
        /// <summary>
        /// Update T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T?> Update(T entity);
        /// <summary>
        /// Delete T Type Entity record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(string id);
        /// <summary>
        /// Get Record for T Type Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetById(string id);
        /// <summary>
        /// Get All Record for T Type Entity
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>?> GetAll();
    }
}
