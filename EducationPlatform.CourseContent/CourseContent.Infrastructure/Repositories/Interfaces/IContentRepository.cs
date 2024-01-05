using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        bool RemoveRange(IEnumerable<T> entities);
    }
}
