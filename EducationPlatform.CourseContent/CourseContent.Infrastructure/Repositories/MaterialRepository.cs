using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories
{
    public class MaterialRepository(IContentRepository<Material> contentRepository) :
        IContentRepository<Material>
    {
        private readonly IContentRepository<Material> _contentRepository = contentRepository;

        public async Task<Material> AddAsync(Material entity)
        {
            return await _contentRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _contentRepository.DeleteAsync(id);
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            return await _contentRepository.GetByIdAsync(id);
        }

        public async Task<Material> UpdateAsync(int id, Material entity)
        {
            return await _contentRepository.UpdateAsync(id, entity);
        }

        public async Task<IEnumerable<Material>> GetAllByCourseAsync(Expression<Func<Material, bool>> filter)
        {
            return await _contentRepository.GetAllByCourseAsync(filter);
        }

        public void RemoveRange(IEnumerable<Material> entities)
        {
            _contentRepository.RemoveRange(entities);
        }

        public void AddFiles(Material entity, string file)
        {
            _contentRepository.AddFiles(entity, file);
        }       
    }
}
