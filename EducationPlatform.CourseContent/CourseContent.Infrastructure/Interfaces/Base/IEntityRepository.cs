using CourseContent.Domain.Interfaces;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Interfaces.Base
{
    public interface IEntityRepository<T> : IMinRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    }
}
