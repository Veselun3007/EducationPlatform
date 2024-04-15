using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentlinkRepository(IMinRepository<Assignmentlink> repository) :
        IMinRepository<Assignmentlink>
    {
        private readonly IMinRepository<Assignmentlink> _repository = repository;


        public async Task<Assignmentlink> AddAsync(Assignmentlink entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
