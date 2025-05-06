using CourseService.Application.Interfaces.Base;
using CourseService.Domain.Base;
using CourseService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories.Generic
{
    internal class Repository<TKey, TEntity> : MinRepository<TKey, TEntity>,
        IRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        public Repository(EducationPlatformContext dbContext) : base(dbContext) { }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TKey id, TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if(existingEntity is not null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            return existingEntity;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            foreach(var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(filter).ToListAsync();
        }

        public virtual async Task<TEntity?> FindAnyAsync(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            foreach(var include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync(filter);
        }
    }
}
