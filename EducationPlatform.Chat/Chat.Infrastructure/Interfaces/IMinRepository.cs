using EPChat.Domain.Interfaces;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IMinRepository<T> where T : class, IEntity
    {
        Task<T?> GetById(int id, params Expression<Func<T, object>>[]? includes);

        Task<T> AddAsync(T entity);

        Task DeleteAsync(int id);
    }
}
