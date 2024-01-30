using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services
{
    public class MaterialService(IUnitOfWork unitOfWork, FilesHelper fileHelper) :
        IOperation<Material>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FilesHelper _fileHelper = fileHelper;


        public async Task<Material> CreateAsync(Material entity, List<IFormFile> files)
        {
            await _unitOfWork.MaterialRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            if (files is not null)
            {
                await AddFilesAsync(entity, files);
            }
            return entity;

        }

        private async Task AddFilesAsync(Material entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                _unitOfWork.MaterialRepository.AddFiles(entity, fileLink);
            }
            await _unitOfWork.CompleteAsync();
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
            var Material = await _unitOfWork.MaterialRepository.GetByIdAsync(id);
            return Material!;
        }

        public async Task RemoveRangeAsync(IEnumerable<Material> entities)
        {
            _unitOfWork.MaterialRepository.RemoveRange(entities);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            var Materialfile = await _unitOfWork
                .MaterialfileRepository.GetByIdAsync(id);

            return await _fileHelper.GetFileLink(Materialfile.MaterialFile!);
        }

        public IQueryable<Material> GetByCourse(int id)
        {
            return _unitOfWork.MaterialRepository.GetByCourse(a => a.CourseId == id);
        }
    }
}
