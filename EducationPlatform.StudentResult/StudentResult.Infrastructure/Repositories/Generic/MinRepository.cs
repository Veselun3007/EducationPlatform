using Microsoft.EntityFrameworkCore;
using StudentResult.Application.Interfaces;
using StudentResult.Domain.Base;
using StudentResult.Infrastructure.Context;
using System.Linq.Expressions;

namespace StudentResult.Infrastructure.Repositories.Generic
{
    public abstract class MinRepository<TKey, TEntity> :
        IMinRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
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

            return query.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
