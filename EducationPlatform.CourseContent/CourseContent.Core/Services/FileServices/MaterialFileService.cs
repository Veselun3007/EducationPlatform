using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.FileServices
{
    public class MaterialFileService : IFileService<MaterialfileOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public MaterialFileService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<MaterialfileOutDTO?> AddFileAsync(IFormFile formFile, int id)
        {
            var fileLink = await _fileService.AddFileAsync(formFile);
            if(fileLink is null)
            {
                return null;
            }
            var materialFile = NativeMapper.ToMaterialFile(id, fileLink);
            var addedFile = await _unitOfWork.MaterialfileRepository.AddAsync(materialFile);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromMaterialFile(addedFile);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var materialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            if(materialFile?.MaterialFile is not null)
            {
                await _fileService.DeleteFileAsync(materialFile.MaterialFile);
            }
            await _unitOfWork.MaterialfileRepository.DeleteAsync(fileId);
        }

        public async Task<string?> GetFileByIdAsync(int fileId)
        {
            var materialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            if(materialFile?.MaterialFile is null)
            {
                return null;
            }
            return await _fileService.GetFileLink(materialFile.MaterialFile);
        }
    }
}
