using EPChat.Domain.Interfaces;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IRepository<T> : IMinRepository<T> where T : class, IEntity
    {

        IQueryable<T> Get();

        IQueryable<T> Get(Expression<Func<T, bool>> filter);

        T? Find(params object[] keyValues);

        void Add(T entity);

        void Update(T entityToUpdate);

        void Delete(params object[] keyValues);

        void Delete(T entityToDelete);

        void DeleteRange(IEnumerable<T> entityToDelete);

    }
}
