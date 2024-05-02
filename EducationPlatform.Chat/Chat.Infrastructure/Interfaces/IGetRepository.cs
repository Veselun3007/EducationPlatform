using System.Linq.Expressions;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IGetRepository<T>
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);

        IQueryable<T> GetQueryable();
    }
}
