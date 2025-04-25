using CourseContent.Domain.Base;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public abstract class EntityRepository<T, TKey> : MinRepository<T, TKey>, IEntityRepository<T, TKey>  
        where T : AggregateRoot<TKey>
    {
        protected EntityRepository(EducationPlatformContext dbContext) : base(dbContext) { }

        public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
