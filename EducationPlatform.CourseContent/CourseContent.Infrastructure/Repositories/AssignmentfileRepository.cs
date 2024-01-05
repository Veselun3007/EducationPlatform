using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentfileRepository(IRepository<Assignmentfile> repository) :
        IRepository<Assignmentfile>
    {
        private readonly IRepository<Assignmentfile> _repository = repository;

        public async Task<Assignmentfile> AddAsync(Assignmentfile entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Assignmentfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Assignmentfile?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Assignmentfile?> UpdateAsync(int id, Assignmentfile entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }
    }
}
