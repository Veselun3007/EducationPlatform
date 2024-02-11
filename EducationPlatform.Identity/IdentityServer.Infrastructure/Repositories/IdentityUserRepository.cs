using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class IdentityAppUserRepository(IRepository<AppUser> repository)
        : IRepository<AppUser>
    {
        private readonly IRepository<AppUser> _repository = repository;

        public async Task<AppUser> AddAsync(AppUser entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<AppUser?> UpdateAsync(AppUser entity, int id)
        {
            return await _repository.UpdateAsync(entity, id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<AppUser?> FindByParamAsync(Expression<Func<AppUser, bool>> filter)
        {
            return await _repository.FindByParamAsync(filter);
        }
    }
}
