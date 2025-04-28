using Chat.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Chat.Core.Interfaces.Infrastructure
{
    public interface IRepository<T, TKey> : IMinRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>?> GetEntitiesAsync(
             Expression<Func<T, bool>>? filter = null,      
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             int? take = null,
             params Expression<Func<T, object>>[]? includes);

        Task<T?> UpdateAsync(TKey id, T entity);
    }
}
