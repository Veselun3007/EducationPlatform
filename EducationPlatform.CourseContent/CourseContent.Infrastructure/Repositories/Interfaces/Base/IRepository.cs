using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Repositories.Interfaces.Base
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> AddAsync(T entity);

        Task<T?> UpdateAsync(int id, T entity);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);
    }
}
