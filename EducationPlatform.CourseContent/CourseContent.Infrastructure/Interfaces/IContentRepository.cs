using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        bool RemoveRange(IEnumerable<T> entities);

        bool AddFiles(T entity, string file);

        IQueryable<T> GetByCourse(Expression<Func<T, bool>> filter);
    }
}
