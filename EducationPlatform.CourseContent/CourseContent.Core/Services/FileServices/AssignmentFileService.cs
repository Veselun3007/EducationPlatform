using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.FileServices
{
    public class AssignmentFileService : IFileServices<AssignmentfileOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public AssignmentFileService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<AssignmentfileOutDTO> AddFileAsync(IFormFile formFile, int id)
        {
            var fileLink = await _fileService.AddFileAsync(formFile);
            Assignmentfile assignmentFile = NativeMapper.ToAssignmentFile(id, fileLink);
            var addedFile = await _unitOfWork.AssignmentfileRepository.AddAsync(assignmentFile);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromAssignmentFile(addedFile);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(fileId);
            if(assignmentFile is not null && assignmentFile.AssignmentFile is not null)
            {
                await _fileService.DeleteFileAsync(assignmentFile.AssignmentFile);
            }
            await _unitOfWork.AssignmentfileRepository.DeleteAsync(fileId);
        }

        public async Task<string?> GetFileByIdAsync(int fileId)
        {
            var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(fileId);
            return await _fileService.GetFileLink(assignmentFile.AssignmentFile);
        }
    }
}
