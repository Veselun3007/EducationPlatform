using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class MaterialfileRepository(IEntityRepository<Materialfile> repository) :
        IEntityRepository<Materialfile>
    {
        private readonly IEntityRepository<Materialfile> _repository = repository;

        public async Task<Materialfile?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
