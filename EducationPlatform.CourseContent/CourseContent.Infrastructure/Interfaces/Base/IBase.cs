using CourseContent.Domain.Interfaces;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IEntityRepository<T, TKey> : IMinRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
    }

    public interface IMinRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(TKey id);
    }
}
