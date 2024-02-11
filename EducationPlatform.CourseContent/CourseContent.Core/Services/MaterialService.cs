using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CourseContent.Core.Services
{
    public class MaterialService(IUnitOfWork unitOfWork, ILogger<MaterialService> logger, 
        FilesHelper fileHelper) : IOperation<Material>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<MaterialService> _logger = logger;
        private readonly FilesHelper _fileHelper = fileHelper;

        public async Task<Material> CreateAsync(Material entity, List<IFormFile> files)
        {
            try
            {
                await _unitOfWork.MaterialRepository.AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                if (files is not null)
                {
                    await AddFilesAsync(entity, files);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding Material.");
            }
            return entity;
        }

        private async Task AddFilesAsync(Material entity, List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var fileLink = await _fileHelper.AddFileAsync(file);
                    _unitOfWork.MaterialRepository.AddFiles(entity, fileLink);
                }
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding files to Material.");
            }
        }

        public async Task<Material> UpdateAsync(int id, Material entity)
        {
            try
            {
                await _unitOfWork.MaterialRepository.UpdateAsync(id, entity);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Material.");
            }
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.MaterialRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Material.");
            }
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            var material = await _unitOfWork.MaterialRepository.GetByIdAsync(id);
            if (material is null)
                return null;

            return material;
        }

        public async Task RemoveRangeAsync(IEnumerable<Material> entities)
        {
            try
            {
                _unitOfWork.MaterialRepository.RemoveRange(entities);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing Material.");
            }
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            var materialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(id);

            if (materialFile is null)
                return null;

            return await _fileHelper.GetFileLink(materialFile.MaterialFile!);
        }

        public async Task<IEnumerable<Material>> GetAllByCourseAsync(int id)
        {
            return await _unitOfWork.MaterialRepository.GetAllByCourseAsync(m => m.CourseId == id);
        }
    }
}
