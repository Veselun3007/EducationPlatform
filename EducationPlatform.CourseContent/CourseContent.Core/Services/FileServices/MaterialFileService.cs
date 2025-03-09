using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.FileServices
{
    public class MaterialFileService : IFileServices<MaterialfileOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public MaterialFileService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<MaterialfileOutDTO> AddFileAsync(IFormFile formFile, int id)
        {
            var fileLink = await _fileHelper.AddFileAsync(formFile);
            Materialfile MaterialFile = MappingHelpers.CreateMaterialFile(id, fileLink);
            var addedFile = await _unitOfWork.MaterialfileRepository.AddAsync(MaterialFile);
            await _unitOfWork.CommitAsync();

            return MaterialfileOutDTO.FromMaterialFile(addedFile);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var MaterialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            if(MaterialFile is not null && MaterialFile.MaterialFile is not null)
            {
                await _fileHelper.DeleteFileAsync(MaterialFile.MaterialFile);
            }
            await _unitOfWork.MaterialfileRepository.DeleteAsync(fileId);
        }

        public async Task<string?> GetFileByIdAsync(int fileId)
        {
            var MaterialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
            return await _fileHelper.GetFileLink(MaterialFile.MaterialFile);
        }
    }
}
