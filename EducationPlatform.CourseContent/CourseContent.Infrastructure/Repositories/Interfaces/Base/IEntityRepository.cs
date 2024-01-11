using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Repositories.Interfaces.Base
{
    public interface IEntityRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(int id);
    }
}
