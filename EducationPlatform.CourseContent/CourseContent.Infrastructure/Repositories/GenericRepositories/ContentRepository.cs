using CourseContent.Domain.Entities;
using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Helpers;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class ContentRepository<T>(EducationPlatformContext dbContext, 
        FilesHelper filesHelper) : IContentRepository<T> where T : class, IAggregateRoot
    {
        private readonly EducationPlatformContext _dbContext = dbContext;
        private readonly FilesHelper _filesHelper = filesHelper;

        public async Task<T> AddAsync(T entity)
        {

            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> AddFiles(T entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileName = await _filesHelper.AddFileAsync(file);

                if (entity is Material materialEntity)
                {
                    var materialFile = new Materialfile
                    {
                        MaterialId = materialEntity.MaterialId,
                        MaterialFile = fileName
                    };

                    _dbContext.Set<Materialfile>().Add(materialFile);
                }
                else if (entity is Assignment assignmentEntity)
                {
                    var assignmentFile = new Assignmentfile
                    {
                        AssignmentId = assignmentEntity.AssignmentId,
                        AssignmentFile = fileName
                    };

                    _dbContext.Set<Assignmentfile>().Add(assignmentFile);
                }
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            _dbContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public bool RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return true;
        }

        public async Task<T?> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _dbContext.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }

    }
}
