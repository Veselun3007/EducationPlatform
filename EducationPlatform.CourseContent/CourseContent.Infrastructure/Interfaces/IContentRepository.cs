using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        Task RemoveRange(List<int> entities);

        void AddFile(T entity, string file);

        void AddLink(T entity, string link);
    }
}
