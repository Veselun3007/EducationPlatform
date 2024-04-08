using EPChat.Domain.Interfaces;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IRepository<T> : IMinRepository<T> where T : class, IEntity
    {
        Task DeleteAsync(int id);

        Task<T?> UpdateAsync(int id, T entity);

        Task RemoveRangeAsync(List<int> entities);       

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);
    }
}
