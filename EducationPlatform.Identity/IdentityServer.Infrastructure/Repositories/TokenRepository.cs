using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class TokenRepository(IBaseRepository<Token> repository)
        : IBaseRepository<Token>
    {
        private readonly IBaseRepository<Token> _repository = repository;

        public async Task<Token> AddAsync(Token entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Token?> FindByParamAsync(Expression<Func<Token, bool>> filter)
        {
            return await _repository.FindByParamAsync(filter);
        }

        public async Task<Token?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
