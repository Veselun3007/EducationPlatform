using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class MateriallinkRepository(IEntityRepository<Materiallink> repository) :
        IEntityRepository<Materiallink>
    {
        private readonly IEntityRepository<Materiallink> _repository = repository;

        public async Task<Materiallink> AddAsync(Materiallink entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Materiallink?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
