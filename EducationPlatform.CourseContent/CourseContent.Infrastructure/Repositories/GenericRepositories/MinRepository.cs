using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class MinRepository<T> : IMinRepository<T> where T : class, IAggregateRoot
    {
        private readonly EducationPlatformContext _dbContext;
        private readonly DbSet<T> _dbSet;

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

