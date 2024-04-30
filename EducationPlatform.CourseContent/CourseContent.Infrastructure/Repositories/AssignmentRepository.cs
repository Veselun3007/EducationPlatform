using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using System.Linq.Expressions;

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

        public async Task<Assignment?> GetByIdAsync(int id, params Expression<Func<Assignment, object>>[] includes)
        {
            return await _contentRepository.GetByIdAsync(id, includes);
        }

        public async Task<Assignment?> UpdateAsync(int id, Assignment entity)
        {
            return await _contentRepository.UpdateAsync(id, entity);
        }

        public async Task<IEnumerable<Assignment>> GetAllByCourseAsync(Expression<Func<Assignment, bool>> filter)
        {
            return await _contentRepository.GetAllByCourseAsync(filter);
        }
        public async Task DeleteAsync(int id)
        {
            await _contentRepository.DeleteAsync(id);
        }

        public async Task RemoveRange(List<int> entities)
        {
            await _contentRepository.RemoveRange(entities);
        }

        public void AddFile(Assignment entity, string file)
        {
            _contentRepository.AddFile(entity, file);
        }

        public void AddLink(Assignment entity, string link)
        {
            _contentRepository.AddLink(entity, link);
        }
    }
}
