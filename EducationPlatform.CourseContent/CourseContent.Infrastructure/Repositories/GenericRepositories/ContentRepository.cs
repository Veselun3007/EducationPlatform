using CourseContent.Domain.Entities;
using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class ContentRepository<T> : IContentRepository<T> where T : class, IAggregateRoot
    {
        private readonly EducationPlatformContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public ContentRepository(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity!;
        }

        public virtual async Task<T> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity is not null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity!;
        }

        public virtual bool RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;
        }

        public virtual bool AddFiles(T entity, string file)
        {
            if (entity is Material materialEntity)
            {
                var materialFile = new Materialfile
                {
                    MaterialId = materialEntity.Id,
                    MaterialFile = file
                };

                _dbContext.Set<Materialfile>().Add(materialFile);
            }
            else if (entity is Assignment assignmentEntity)
            {
                var assignmentFile = new Assignmentfile
                {
                    AssignmentId = assignmentEntity.Id,
                    AssignmentFile = file
                };

                _dbContext.Set<Assignmentfile>().Add(assignmentFile);
            }

            return true;
        }

        public IQueryable<T> GetByCourse(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter);
        }
    }
}
