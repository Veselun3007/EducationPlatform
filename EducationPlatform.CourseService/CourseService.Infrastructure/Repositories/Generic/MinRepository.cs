using CourseService.Domain.Base;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories.Generic
{
    public abstract class MinRepository<TKey, TEntity> :
        IMinRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        protected readonly EducationPlatformContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public MinRepository(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
