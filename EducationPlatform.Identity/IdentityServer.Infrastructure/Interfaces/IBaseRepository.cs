using System.Linq.Expressions;

namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task DeleteAsync(int id);

        Task<T?> FindByParamAsync(Expression<Func<T, bool>> filter);
    }
}
