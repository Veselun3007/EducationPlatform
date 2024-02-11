using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CourseContent.Core.Services
{
    public class AssignmentService(IUnitOfWork unitOfWork,
        ILogger<AssignmentService> logger, FilesHelper fileHelper) : IOperation<Assignment>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AssignmentService> _logger = logger;
        private readonly FilesHelper _fileHelper = fileHelper;

        public async Task<Assignment> CreateAsync(Assignment entity, List<IFormFile> files)
        {
            try
            {
                await _unitOfWork.AssignmentRepository.AddAsync(entity);
                await _unitOfWork.CompleteAsync();
                if (files is not null && files.Count > 0)
                {
                    await AddFilesAsync(entity, files);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding Assignment.");
            }
            return entity;
        }

        private async Task AddFilesAsync(Assignment entity, List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var fileLink = await _fileHelper.AddFileAsync(file);
                    _unitOfWork.AssignmentRepository.AddFiles(entity, fileLink);
                }
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding files to Assignment.");
            }
        }

        public async Task<Assignment> UpdateAsync(int id, Assignment entity)
        {
            try
            {
                await _unitOfWork.AssignmentRepository.UpdateAsync(id, entity);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Assignment.");
            }
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.AssignmentRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Assignment.");
            }
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            var assignment = await _unitOfWork.AssignmentRepository.GetByIdAsync(id);
            if (assignment is null)
                return null;

            return assignment;
        }

        public async Task RemoveRangeAsync(IEnumerable<Assignment> entities)
        {
            try
            {
                _unitOfWork.AssignmentRepository.RemoveRange(entities);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing Assignment.");
            }
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            var assignmentfile = await _unitOfWork
                .AssignmentfileRepository.GetByIdAsync(id);

            if (assignmentfile is null)
                return null;

            return await _fileHelper.GetFileLink(assignmentfile.AssignmentFile!);
        }

        public async Task<IEnumerable<Assignment>> GetAllByCourseAsync(int id)
        {
            return await _unitOfWork.AssignmentRepository.GetAllByCourseAsync(a => a.CourseId == id);
        }
    }
}
