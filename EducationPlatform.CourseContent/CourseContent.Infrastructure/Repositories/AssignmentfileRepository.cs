using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentfileRepository(IEntityRepository<Assignmentfile> repository) :
        IEntityRepository<Assignmentfile>
    {
        private readonly IEntityRepository<Assignmentfile> _repository = repository;

        public async Task<Assignmentfile?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
