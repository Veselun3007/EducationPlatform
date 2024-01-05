using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Repositories.Interfaces;

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

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _contentRepository.GetByIdAsync(id);
        }

        public async Task<Assignment?> UpdateAsync(int id, Assignment entity)
        {
            return await _contentRepository.UpdateAsync(id, entity);
        }

        public bool RemoveRange(IEnumerable<Assignment> entities)
        {
            return _contentRepository.RemoveRange(entities);
        }
    }
}
