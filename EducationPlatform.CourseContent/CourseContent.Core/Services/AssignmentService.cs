using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;

namespace CourseContent.Core.Services
{
    public class AssignmentService(IUnitOfWork unitOfWork) :
        IOperation<Assignment>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Assignment> CreateAsync(Assignment entity)
        {
            await _unitOfWork.AssignmentRepository.AddAsync(entity);

            if(entity.AssignmentFiles != null)
                await _unitOfWork.AssignmentRepository
                    .AddFiles(entity, entity.AssignmentFiles);

            await _unitOfWork.CompleteAsync();
            return entity;

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
            return assignment ?? throw new InvalidOperationException("Assignment " +
                "not found.");
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

            return assignmentfile != null ? assignmentfile.AssignmentFile : 
                throw new InvalidOperationException("AssignmentFile " +
                "not found for the specified id.");
        }
    }
}
