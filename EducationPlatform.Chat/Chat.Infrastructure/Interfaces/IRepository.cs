using EPChat.Domain.Interfaces;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IRepository<T> : IMinRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);

        IQueryable<T> GetQueryable();

        Task<T?> UpdateAsync(int id, T entity);
    }
}
