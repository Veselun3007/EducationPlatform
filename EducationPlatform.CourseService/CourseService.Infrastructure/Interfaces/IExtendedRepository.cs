using CourseService.Domain.Base;
using CourseService.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Interfaces
{
    public interface IExtendedRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
    }
}
