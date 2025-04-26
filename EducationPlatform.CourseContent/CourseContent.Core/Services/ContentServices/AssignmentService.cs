using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.ContentServices
{
    public class AssignmentService : IContentServices<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public AssignmentService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<IEnumerable<AssignmentOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var assignments = await _unitOfWork.AssignmentRepository
                .FindAllByAsync(m => m.CourseId == courseId);
            return assignments.Select(NativeMapper.FromAssignment).ToList();
        }

        public async Task<AssignmentOutDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.AssignmentRepository
                .GetByIdAsync(id, a => a.Assignmentfiles, a => a.Assignmentlinks);
            return NativeMapper.FromAssignment(entity);
        }

        public async Task<AssignmentOutDTO?> CreateAsync(AssignmentDTO entity)
        {
            var assignment = NativeMapper.ToAssignment(entity);
            await _unitOfWork.AssignmentRepository.AddAsync(assignment);
            await _unitOfWork.CommitAsync();

            if(entity.AssignmentFiles is not null)
            {
                await AddFilesAsync(assignment, entity.AssignmentFiles);
            }
            if(entity.AssignmentLinks is not null)
            {
                await AddLinksAsync(assignment, entity.AssignmentLinks);
            }
            return NativeMapper.FromAssignment(assignment);
        }

        private async Task AddFilesAsync(Assignment entity, List<IFormFile> files)
        {
            foreach(var file in files)
            {
                var fileLink = await _fileService.AddFileAsync(file);
                await _unitOfWork.AssignmentfileRepository.AddAsync(NativeMapper.ToAssignmentFile(entity.Id, fileLink));
            }
            await _unitOfWork.CommitAsync();
        }

        private async Task AddLinksAsync(Assignment entity, List<string> links)
        {
            foreach(var link in links)
            {
                await _unitOfWork.AssignmentlinkRepository.AddAsync(NativeMapper.ToAssignmentLink(link, entity.Id));
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<AssignmentOutDTO?> UpdateAsync(AssignmentUpdateDTO entity, int id)
        {
            var assignment = NativeMapper.ToAssignment(entity);
            await _unitOfWork.AssignmentRepository.UpdateAsync(id, assignment);
            await _unitOfWork.CommitAsync();
            var updatedAssignment = await _unitOfWork.AssignmentRepository
                .GetByIdAsync(id, a => a.Assignmentfiles, a => a.Assignmentlinks);
            return NativeMapper.FromAssignment(updatedAssignment);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.AssignmentRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(List<int> entities)
        {
            await _unitOfWork.AssignmentRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }
    }
}

