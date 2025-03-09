using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services.ContentServices
{
    public class AssignmentService : IContentServices<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public AssignmentService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<IEnumerable<AssignmentOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var assignments = await _unitOfWork.AssignmentRepository
                .GetAllByCourseAsync(m => m.CourseId == courseId);
            return assignments.Select(AssignmentOutDTO.FromAssignment).ToList();
        }

        public async Task<AssignmentOutDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.AssignmentRepository
                .GetByIdAsync(id, a => a.Assignmentfiles, a => a.Assignmentlinks);
            return AssignmentOutDTO.FromAssignment(entity);
        }

        public async Task<AssignmentOutDTO?> CreateAsync(AssignmentDTO entity)
        {
            var assignment = AssignmentDTO.FromAssignmentDto(entity);
            await _unitOfWork.AssignmentRepository.AddAsync(assignment);
            await _unitOfWork.CommitAsync();

            if (entity.AssignmentFiles is not null)
            {
                await AddFilesAsync(assignment, entity.AssignmentFiles);
            }
            if (entity.AssignmentLinks is not null)
            {
                await AddLinksAsync(assignment, entity.AssignmentLinks);
            }
            return AssignmentOutDTO.FromAssignment(assignment);
        }

        private async Task AddFilesAsync(Assignment entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                await _unitOfWork.AssignmentfileRepository.AddAsync(MappingHelpers.CreateAssignmentFile(entity.Id, fileLink));
            }
            await _unitOfWork.CommitAsync();
        }

        private async Task AddLinksAsync(Assignment entity, List<string> links)
        {
            foreach (var link in links)
            {
                await _unitOfWork.AssignmentlinkRepository.AddAsync(MappingHelpers.CreateAssignmentLink(link, entity.Id));
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<AssignmentOutDTO?> UpdateAsync(AssignmentUpdateDTO entity, int id)
        {
            var assignment = AssignmentUpdateDTO.FromAssignmentUpdateDto(entity);
            await _unitOfWork.AssignmentRepository.UpdateAsync(id, assignment);
            await _unitOfWork.CommitAsync();
            var updatedAssignment = await _unitOfWork.AssignmentRepository
                .GetByIdAsync(id, a => a.Assignmentfiles, a => a.Assignmentlinks);
            return AssignmentOutDTO.FromAssignment(updatedAssignment);
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

