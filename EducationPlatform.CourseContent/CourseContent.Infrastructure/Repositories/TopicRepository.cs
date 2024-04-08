using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories
{
    public class TopicRepository(IRepository<Topic> repository) : IRepository<Topic>
    {
        private readonly IRepository<Topic> _repository = repository;

        public async Task<Topic> AddAsync(Topic entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Topic>> GetAllByCourseAsync(Expression<Func<Topic, bool>> filter)
        {
            return await _repository.GetAllByCourseAsync(filter);
        }

        public async Task<Topic?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Topic?> UpdateAsync(int id, Topic entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }
    }
}
