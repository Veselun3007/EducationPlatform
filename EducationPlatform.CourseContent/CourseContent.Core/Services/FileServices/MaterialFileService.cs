using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;
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

        public async Task<MaterialfileOutDTO> AddFileAsync(IFormFile formFile, int id)
        {
            var fileLink = await _fileService.AddFileAsync(formFile);
            Materialfile MaterialFile = NativeMapper.ToMaterialFile(id, fileLink);
            var addedFile = await _unitOfWork.MaterialfileRepository.AddAsync(MaterialFile);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromMaterialFile(addedFile);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var MaterialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            if(MaterialFile is not null && MaterialFile.MaterialFile is not null)
            {
                await _fileService.DeleteFileAsync(MaterialFile.MaterialFile);
            }
            await _unitOfWork.MaterialfileRepository.DeleteAsync(fileId);
        }

        public async Task<string?> GetFileByIdAsync(int fileId)
        {
            var MaterialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            return await _fileService.GetFileLink(MaterialFile.MaterialFile);
        }
    }
}
