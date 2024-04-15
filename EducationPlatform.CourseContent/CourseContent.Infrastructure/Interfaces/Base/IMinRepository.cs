using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IMinRepository<T> where T : IAggregateRoot
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(int id);
    }
}
