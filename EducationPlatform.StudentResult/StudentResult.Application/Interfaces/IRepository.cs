using StudentResult.Domain.Base;
using System.Linq.Expressions;

namespace StudentResult.Application.Interfaces
{
    public interface IMinRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    }

    public interface IRepository<TKey, TEntity> : IMinRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        Task DeleteAsync(TKey id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TKey id, TEntity entity);

        Task<TEntity?> FindAnyAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);
    }
}
