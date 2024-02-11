using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class UserRepository(IRepository<User> repository) :
        IRepository<User>
    {
        private readonly IRepository<User> _repository = repository;

        public async Task<User> AddAsync(User entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<User?> UpdateAsync(User entity, int id)
        {
            return await _repository.UpdateAsync(entity, id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> FindByParamAsync(Expression<Func<User, bool>> filter)
        {
            return await _repository.FindByParamAsync(filter);
        }
    }
}
