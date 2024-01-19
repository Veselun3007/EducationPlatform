using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IContentRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        bool RemoveRange(IEnumerable<T> entities);

        Task<bool> AddFiles(T entity, List<IFormFile> files);
    }
}
