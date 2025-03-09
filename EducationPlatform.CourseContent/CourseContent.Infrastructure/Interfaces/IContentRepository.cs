using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T, TKey> : IEntityRepository<T, TKey> where T : IAggregateRoot<TKey> 
    {
        Task RemoveRange(List<TKey> entities);

        Task<T?> UpdateAsync(TKey id, T entity);

        Task<IEnumerable<T>> GetAllByCourseAsync(Expression<Func<T, bool>> filter);
    }
}
