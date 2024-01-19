using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

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

        public async Task<bool> DeleteAsync(int id)
        {
            return await _contentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            return await _contentRepository.GetAllAsync();
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            return await _contentRepository.GetByIdAsync(id);
        }

        public async Task<Material?> UpdateAsync(int id, Material entity)
        {
            return await _contentRepository.UpdateAsync(id, entity);
        }

        public bool RemoveRange(IEnumerable<Material> entities)
        {
            return _contentRepository.RemoveRange(entities);
        }

        public async Task<bool> AddFiles(Material entity, List<IFormFile> files)
        {
            return await _contentRepository.AddFiles(entity, files);
        }
    }
}
