using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;

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

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
