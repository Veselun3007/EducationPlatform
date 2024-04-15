using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class MateriallinkRepository(IMinRepository<Materiallink> repository) :
        IMinRepository<Materiallink>
    {
        private readonly IMinRepository<Materiallink> _repository = repository;

        public async Task<Materiallink> AddAsync(Materiallink entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
