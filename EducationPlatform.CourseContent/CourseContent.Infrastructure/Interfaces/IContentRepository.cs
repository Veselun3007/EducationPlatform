using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        Task RemoveRange(List<int> entities);

        void AddFiles(T entity, string file);
    }
}
