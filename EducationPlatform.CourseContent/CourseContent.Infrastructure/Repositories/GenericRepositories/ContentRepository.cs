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

            return existingEntity!;
        }

        public async Task<IEnumerable<T>> GetAllByCourseAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity is not null) 
            { 
                _dbSet.Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void AddFiles(T entity, string file)
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
        }
    }
}
