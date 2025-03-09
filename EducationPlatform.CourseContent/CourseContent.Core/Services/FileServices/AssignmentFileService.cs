using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.FileServices
{
    public class AssignmentFileService : IFileServices<AssignmentfileOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public AssignmentFileService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<AssignmentfileOutDTO> AddFileAsync(IFormFile formFile, int id)
        {
            var fileLink = await _fileHelper.AddFileAsync(formFile);
            Assignmentfile assignmentFile = MappingHelpers.CreateAssignmentFile(id, fileLink);
            var addedFile = await _unitOfWork.AssignmentfileRepository.AddAsync(assignmentFile);
            await _unitOfWork.CommitAsync();

            return AssignmentfileOutDTO.FromAssignmentFile(addedFile);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(fileId);
            if(assignmentFile is not null && assignmentFile.AssignmentFile is not null)
            {
                await _fileHelper.DeleteFileAsync(assignmentFile.AssignmentFile);
            }
            await _unitOfWork.AssignmentfileRepository.DeleteAsync(fileId);
        }

        public async Task<string?> GetFileByIdAsync(int fileId)
        {
            var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(fileId);
            return await _fileHelper.GetFileLink(assignmentFile.AssignmentFile);
        }
    }
}
