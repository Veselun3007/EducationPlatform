using CourseContent.Domain.Base;
using System.Linq.Expressions;

namespace CourseContent.Core.Interfaces.Base
{
    public interface IEntityRepository<T, TKey> : IMinRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
    }

    public interface IMinRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(TKey id);
    }
}
