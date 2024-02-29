using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        void RemoveRange(IEnumerable<T> entities);

        void AddFiles(T entity, string file);
    }
}
