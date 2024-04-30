using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories
{
    public class MaterialfileRepository(IEntityRepository<Materialfile> repository) :
        IEntityRepository<Materialfile>
    {
        private readonly IEntityRepository<Materialfile> _repository = repository;

        public async Task<Materialfile> AddAsync(Materialfile entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Materialfile?> GetByIdAsync(int id, params Expression<Func<Materialfile, object>>[] includes)
        {
            return await _repository.GetByIdAsync(id, includes);
        }
    }
}
