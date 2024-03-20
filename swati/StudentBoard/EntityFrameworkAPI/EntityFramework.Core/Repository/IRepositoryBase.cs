using EntityFrameworkAPI.EntityFramework.Core.Common;
using EntityFrameworkAPI.EntityFrameworkAPI.Core.Common;
using System.Linq.Expressions;

namespace EntityFrameworkAPI.EntityFrameworkAPI.Core.Repository
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
