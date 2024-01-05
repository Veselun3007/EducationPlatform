using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class MaterialfileRepository(IRepository<Materialfile> repository) :
        IRepository<Materialfile>
    {
        private readonly IRepository<Materialfile> _repository = repository;

        public async Task<Materialfile> AddAsync(Materialfile entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Materialfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Materialfile?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Materialfile?> UpdateAsync(int id, Materialfile entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }
    }
}
