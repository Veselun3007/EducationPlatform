using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.ContentServices
{
    public class MaterialService : IContentServices<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public MaterialService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<IEnumerable<MaterialOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var Materials = await _unitOfWork.MaterialRepository
                .FindAllByAsync(m => m.CourseId == courseId);
            return Materials.Select(MaterialOutDTO.FromMaterial).ToList();
        }

        public async Task<MaterialOutDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.MaterialRepository
                .GetByIdAsync(id, a => a.Materialfiles, a => a.Materiallinks);
            return MaterialOutDTO.FromMaterial(entity);
        }

        public async Task<MaterialOutDTO?> CreateAsync(MaterialDTO entity)
        {
            var Material = MaterialDTO.FromMaterialDto(entity);
            await _unitOfWork.MaterialRepository.AddAsync(Material);
            await _unitOfWork.CommitAsync();

            if (entity.MaterialFiles is not null)
            {
                await AddFilesAsync(Material, entity.MaterialFiles);
            }
            if (entity.MaterialLinks is not null)
            {
                await AddLinksAsync(Material, entity.MaterialLinks);
            }
            return MaterialOutDTO.FromMaterial(Material);
        }

        private async Task AddFilesAsync(Material entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                await _unitOfWork.MaterialfileRepository.AddAsync(MappingHelpers.CreateMaterialFile(entity.Id, fileLink));
            }
            await _unitOfWork.CommitAsync();
        }

        private async Task AddLinksAsync(Material entity, List<string> links)
        {
            foreach (var link in links)
            {
                await _unitOfWork.MateriallinkRepository.AddAsync(MappingHelpers.CreateMaterialLink(link, entity.Id));
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<MaterialOutDTO?> UpdateAsync(MaterialUpdateDTO entity, int id)
        {
            var Material = MaterialUpdateDTO.FromMaterialUpdateDto(entity);
            await _unitOfWork.MaterialRepository.UpdateAsync(id, Material);
            await _unitOfWork.CommitAsync();
            var updatedMaterial = await _unitOfWork.MaterialRepository
                .GetByIdAsync(id, a => a.Materialfiles, a => a.Materiallinks);
            return MaterialOutDTO.FromMaterial(updatedMaterial);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.MaterialRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(List<int> entities)
        {
            await _unitOfWork.MaterialRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }
    }
}
