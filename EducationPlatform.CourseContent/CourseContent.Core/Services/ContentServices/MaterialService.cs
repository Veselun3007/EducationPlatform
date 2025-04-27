using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.ContentServices
{
    public class MaterialService : IContentServices<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public MaterialService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<IEnumerable<MaterialOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var Materials = await _unitOfWork.MaterialRepository
                .FindAllByAsync(m => m.CourseId == courseId);
            return Materials.Select(NativeMapper.FromMaterial);
        }

        public async Task<MaterialOutDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.MaterialRepository
                .GetByIdAsync(id, a => a.Materialfiles, a => a.Materiallinks);
            return NativeMapper.FromMaterial(entity);
        }

        public async Task<MaterialOutDTO?> CreateAsync(MaterialDTO entity)
        {
            var material = NativeMapper.ToMaterial(entity);
            await _unitOfWork.MaterialRepository.AddAsync(material);
            await _unitOfWork.CommitAsync();

            if(entity.MaterialFiles is not null)
            {
                await AddFilesAsync(material, entity.MaterialFiles);
            }
            if(entity.MaterialLinks is not null)
            {
                await AddLinksAsync(material, entity.MaterialLinks);
            }
            return NativeMapper.FromMaterial(material);
        }

        private async Task AddFilesAsync(Material entity, List<IFormFile> files)
        {
            foreach(var file in files)
            {
                var fileLink = await _fileService.AddFileAsync(file);
                await _unitOfWork.MaterialfileRepository.AddAsync(NativeMapper.ToMaterialFile(entity.Id, fileLink));
            }
            await _unitOfWork.CommitAsync();
        }

        private async Task AddLinksAsync(Material entity, List<string> links)
        {
            foreach(var link in links)
            {
                await _unitOfWork.MateriallinkRepository.AddAsync(NativeMapper.ToMaterialLink(link, entity.Id));
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<MaterialOutDTO?> UpdateAsync(MaterialUpdateDTO entity, int id)
        {
            var material = NativeMapper.ToMaterial(entity);
            await _unitOfWork.MaterialRepository.UpdateAsync(id, material);
            await _unitOfWork.CommitAsync();
            var updatedMaterial = await _unitOfWork.MaterialRepository
                .GetByIdAsync(id, a => a.Materialfiles, a => a.Materiallinks);
            return NativeMapper.FromMaterial(updatedMaterial);
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
