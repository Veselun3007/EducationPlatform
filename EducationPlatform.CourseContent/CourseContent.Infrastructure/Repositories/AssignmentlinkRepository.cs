using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentlinkRepository(IEntityRepository<Assignmentlink> repository) :
        IEntityRepository<Assignmentlink>
    {
        private readonly IEntityRepository<Assignmentlink> _repository = repository;


        public async Task<Assignmentlink> AddAsync(Assignmentlink entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Assignmentlink?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
