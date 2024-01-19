using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;

namespace CourseContent.Core.Services
{
    public class MaterialService(IUnitOfWork unitOfWork) :
        IOperation<Material>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Material> CreateAsync(Material entity)
        {
            await _unitOfWork.MaterialRepository.AddAsync(entity);

            if (entity.MaterialFiles != null)
                await _unitOfWork.MaterialRepository
                    .AddFiles(entity, entity.MaterialFiles);

            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public async Task<Material> UpdateAsync(int id, Material entity)
        {
            await _unitOfWork.MaterialRepository.UpdateAsync(id, entity);
            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.MaterialRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            return await _unitOfWork.MaterialRepository.GetAllAsync();
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            var material = await _unitOfWork.MaterialRepository.GetByIdAsync(id);
            return material ?? throw new InvalidOperationException("Assignment " +
                "not found.");
        }

        public async Task RemoveRangeAsync(IEnumerable<Material> entities)
        {
            _unitOfWork.MaterialRepository.RemoveRange(entities);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            var materialfile = await _unitOfWork
                .MaterialfileRepository.GetByIdAsync(id);

            return materialfile != null ? materialfile.MaterialFile : 
                throw new InvalidOperationException("MaterialFile " +
                "not found for the specified id.");
        }
    }
}
