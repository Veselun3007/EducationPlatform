using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using Identity.Core.Models;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services
{
    public class AssignmentService(IUnitOfWork unitOfWork, FileHelper fileHelper) : 
        IOperation<AssignmentOutDTO, Error, AssignmentDTO, AssignmentfileOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FileHelper _fileHelper = fileHelper;

        public async Task<Result<AssignmentOutDTO, Error>> CreateAsync(AssignmentDTO entity)
        {
            var assignment = AssignmentDTO.FromAssignmentDto(entity);
            await _unitOfWork.AssignmentRepository.AddAsync(assignment);
            await _unitOfWork.CompleteAsync();

            if (entity.AssignmentFiles is not null)
            {
                await AddFilesAsync(assignment, entity.AssignmentFiles);
            }
            if (entity.AssignmentLinks is not null)
            {
                await AddLinksAsync(assignment, entity.AssignmentLinks);
            }
            return Result.Success<AssignmentOutDTO, Error>(AssignmentOutDTO.FromAssignment(assignment));
        }

        private async Task AddFilesAsync(Assignment entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                _unitOfWork.AssignmentRepository.AddFile(entity, fileLink);
            }
            await _unitOfWork.CompleteAsync();
        }

        private async Task AddLinksAsync(Assignment entity, List<string> links)
        {
            foreach (var link in links)
            {
                _unitOfWork.AssignmentRepository.AddLink(entity, link);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Result<AssignmentOutDTO, Error>> UpdateAsync(AssignmentDTO entity, int id)
        {
            try
            {
                var assignment = AssignmentDTO.FromAssignmentDto(entity);               
                assignment.IsEdited = true;
                assignment.EditedTime = DateTime.UtcNow;
                await _unitOfWork.AssignmentRepository.UpdateAsync(id, assignment);
                await _unitOfWork.CompleteAsync();

                return Result.Success<AssignmentOutDTO, Error>(AssignmentOutDTO.FromAssignment(assignment));
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<AssignmentOutDTO, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.AssignmentRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> RemoveRangeAsync(List<int> entities)
        {
            if (entities.Count == 0)
            {
                return Result.Failure<string, Error>(Errors.General.NotRecords());
            }

            await _unitOfWork.AssignmentRepository.RemoveRange(entities);
            await _unitOfWork.CompleteAsync();
            return Result.Success<string, Error>("Deleted was successful");
        }

        public async Task<Result<string, Error>> DeleteFileAsync(int id)
        {
            try
            {
                var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(id);
                if (assignmentFile is not null && assignmentFile.AssignmentFile is not null)
                {
                    await _fileHelper.DeleteFileAsync(assignmentFile.AssignmentFile);
                }

                await _unitOfWork.AssignmentfileRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> DeleteLinkAsync(int linkId)
        {
            try
            {
                await _unitOfWork.AssignmentfileRepository.DeleteAsync(linkId);
                await _unitOfWork.CompleteAsync();                            
                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<AssignmentfileOutDTO, Error>> AddFileAsync(IFormFile file, int id)
        {
            var fileLink = await _fileHelper.AddFileAsync(file);
            Assignmentfile assignmentFile = new ()
            {
                AssignmentId = id,
                AssignmentFile = fileLink
            };
            var addedFile = await _unitOfWork.AssignmentfileRepository.AddAsync(assignmentFile);
            await _unitOfWork.CompleteAsync();

            return Result.Success<AssignmentfileOutDTO, Error>(AssignmentfileOutDTO.FromAssignmentFile(addedFile));
        }

        public async Task<Result<string, Error>> AddLinkAsync(string link, int id)
        {
            Assignmentlink assignmentLink = new()
            {
                AssignmentId = id,
                AssignmentLink = link
            };
            var addedLink = await _unitOfWork.AssignmentlinkRepository.AddAsync(assignmentLink);
            await _unitOfWork.CompleteAsync();

            if (addedLink.AssignmentLink is not null)
            {
                return Result.Success<string, Error>(addedLink.AssignmentLink);
            }
            else
            {
                return Result.Failure<string, Error>(Errors.General.NotAdded());
            }
        }

        public async Task<Result<AssignmentOutDTO, Error>> GetByIdAsync(int id)
        {

            var entity = await _unitOfWork.AssignmentRepository.GetByIdAsync(id);
            if (entity is null)
            {
                return Result.Failure<AssignmentOutDTO, Error>(Errors.General.NotFound());
            }
            return Result.Success<AssignmentOutDTO, Error>(AssignmentOutDTO.FromAssignment(entity));
        }

        public async Task<Result<string, Error>> GetFileByIdAsync(int id)
        {
            var assignmentFile = await _unitOfWork.AssignmentfileRepository.GetByIdAsync(id);

            if (assignmentFile is null || assignmentFile.AssignmentFile is null)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
            return Result.Success<string, Error>(await _fileHelper
                .GetFileLink(assignmentFile.AssignmentFile));
        }

        public async Task<IEnumerable<AssignmentOutDTO>> GetAllByCourseAsync(int id)
        {
            var assignments = await _unitOfWork.AssignmentRepository
                .GetAllByCourseAsync(m => m.CourseId == id);
            return assignments.Select(AssignmentOutDTO.FromAssignment).ToList();
        }
    }
}
