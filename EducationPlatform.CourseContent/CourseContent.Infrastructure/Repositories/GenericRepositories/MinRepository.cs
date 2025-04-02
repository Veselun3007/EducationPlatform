using CourseContent.Domain.Base;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public abstract class MinRepository<T, TKey> : IMinRepository<T, TKey> 
        where T : AggregateRoot<TKey>
    {
        protected readonly EducationPlatformContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public MinRepository(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}

