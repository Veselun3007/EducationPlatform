using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public abstract class ContentRepository<T, TKey> : EntityRepository<T, TKey>, IContentRepository<T, TKey> 
        where T : class, IAggregateRoot<TKey> 
    {
        protected ContentRepository(EducationPlatformContext dbContext) : base(dbContext) { }
     
        public virtual async Task RemoveRange(List<TKey> entities)
        {
            var items = await _dbSet.Where(x => entities.Contains(x.Id)).ToListAsync();
            _dbSet.RemoveRange(items);
        }

        public virtual async Task<T?> UpdateAsync(TKey id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if(existingEntity is not null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            return existingEntity;
        }

        public async Task<IEnumerable<T>> GetAllByCourseAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }
    }
}
