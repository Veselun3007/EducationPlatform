using CourseService.Domain.Entities;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Interfaces {
    public interface IRepository<TEntity> where TEntity : class  {
        Task<TEntity> GetByIdAsync(object id);

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> FindEnumerable(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
