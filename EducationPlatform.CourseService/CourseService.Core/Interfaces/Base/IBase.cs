using CourseService.Domain.Base;
using System.Linq.Expressions;

namespace CourseService.Application.Interfaces.Base
{
    public interface IMinRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    }

    public interface IRepository<TKey, TEntity> : IMinRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        Task DeleteAsync(TKey id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TKey id, TEntity entity);

        Task<TEntity?> FindAnyAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);
    }
}
