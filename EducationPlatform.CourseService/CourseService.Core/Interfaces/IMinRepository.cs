using CourseService.Domain.Base;
using System.Linq.Expressions;

namespace CourseService.Application.Interfaces
{
    public interface IMinRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    }
}
