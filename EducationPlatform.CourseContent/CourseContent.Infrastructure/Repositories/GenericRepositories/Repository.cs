using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {

        private readonly EducationPlatformContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T?> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity is not null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }

        public async Task<IEnumerable<T>> GetAllByCourseAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

    }
}
