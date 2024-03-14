using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IEntityRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task DeleteAsync(int id);
    }
}
