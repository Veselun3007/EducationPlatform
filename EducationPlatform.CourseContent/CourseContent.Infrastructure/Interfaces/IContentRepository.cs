using CourseContent.Domain.Base;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T, TKey> : IEntityRepository<T, TKey> where T : AggregateRoot<TKey> 
    {
        Task RemoveRange(List<TKey> entities);

        Task<T?> UpdateAsync(TKey id, T entity);

        Task<IEnumerable<T>> FindAllByAsync(Expression<Func<T, bool>> filter);
    }
}
