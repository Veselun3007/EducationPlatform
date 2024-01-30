using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using System.Linq.Expressions;
using static Amazon.S3.Util.S3EventNotification;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentRepository(IContentRepository<Assignment> contentRepository) : 
        IContentRepository<Assignment>
    {
        private readonly IContentRepository<Assignment> _contentRepository = contentRepository;

        public async Task<Assignment> AddAsync(Assignment entity)
        {
           return await _contentRepository.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _contentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _contentRepository.GetAllAsync();
        }

        public async Task<Assignment> GetByIdAsync(int id)
        {
            return await _contentRepository.GetByIdAsync(id);
        }

        public async Task<Assignment> UpdateAsync(int id, Assignment entity)
        {
            return await _contentRepository.UpdateAsync(id, entity);
        }

        public bool RemoveRange(IEnumerable<Assignment> entities)
        {
            return _contentRepository.RemoveRange(entities);
        }

        public bool AddFiles(Assignment entity, string file)
        {
            return _contentRepository.AddFiles(entity, file);
        }

        public IQueryable<Assignment> GetByCourse(Expression<Func<Assignment, bool>> filter)
        {
            return _contentRepository.GetByCourse(filter);
        }
    }
}
