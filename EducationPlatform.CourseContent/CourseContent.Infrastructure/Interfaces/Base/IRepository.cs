using CourseContent.Domain.Interfaces;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IRepository<T> : IEntityRepository<T>
        where T : IAggregateRoot
    {
        Task<T?> UpdateAsync(int id, T entity);

        Task<IEnumerable<T>> GetAllByCourseAsync(Expression<Func<T, bool>> filter);
    }
}
