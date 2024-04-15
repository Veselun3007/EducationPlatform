using CourseContent.Domain.Interfaces;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IEntityRepository<T> : IMinRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(int id);       
    }
}
