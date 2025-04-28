using Chat.Domain.Interfaces;
using System.Linq.Expressions;

namespace Chat.Core.Interfaces.Infrastructure
{
    public interface IMinRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[]? includes);

        Task<T> AddAsync(T entity);

        Task DeleteAsync(TKey id);
    }
}
