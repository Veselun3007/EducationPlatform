using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services
{
    public class AssignmentService(IUnitOfWork unitOfWork,
        FilesHelper fileHelper) : IOperation<Assignment>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FilesHelper _fileHelper = fileHelper;

        public async Task<Assignment> CreateAsync(Assignment entity, List<IFormFile> files)
        {
            await _unitOfWork.AssignmentRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            if (files is not null)
            {
                await AddFilesAsync(entity, files);
            }            
            return entity;

        }

        private async Task AddFilesAsync(Assignment entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                _unitOfWork.AssignmentRepository.AddFiles(entity, fileLink);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Assignment> UpdateAsync(int id, Assignment entity)
        {
            await _unitOfWork.AssignmentRepository.UpdateAsync(id, entity);
            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.AssignmentRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _unitOfWork.AssignmentRepository.GetAllAsync();
        }

        public async Task<Assignment> GetByIdAsync(int id)
        {
            var assignment = await _unitOfWork.AssignmentRepository.GetByIdAsync(id);
            return assignment!;
        }

        public async Task RemoveRangeAsync(IEnumerable<Assignment> entities)
        {
            _unitOfWork.AssignmentRepository.RemoveRange(entities);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            var assignmentfile = await _unitOfWork
                .AssignmentfileRepository.GetByIdAsync(id);

            return await _fileHelper.GetFileLink(assignmentfile.AssignmentFile!);
        }

        public IQueryable<Assignment> GetByCourse(int id)
        {
            return _unitOfWork.AssignmentRepository.GetByCourse(a => a.CourseId == id);
        }
    }
}
