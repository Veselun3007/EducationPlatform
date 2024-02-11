using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IRepository<T> : IEntityRepository<T>
        where T : IAggregateRoot
    {
        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(int id, T entity);

        Task DeleteAsync(int id);
    }
}
