using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Repositories;

namespace IdentityServer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _businessContext;
        private readonly IdentityDBContext _identityContext;

        public UnitOfWork(EducationPlatformContext businessContext,
            IdentityDBContext identityContext)
        {
            _businessContext = businessContext;
            _identityContext = identityContext;
            UserRepository = new Repository<User>(_businessContext);
            IdentityRepository = new Repository<AppUser>(_identityContext);
            TokenRepository = new BaseRepository<Token>(_identityContext);
        }

        public IRepository<User> UserRepository { get; private set; }
        public IRepository<AppUser> IdentityRepository { get; private set; }
        public IBaseRepository<Token> TokenRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            int businessChanges = await _businessContext.SaveChangesAsync();
            int identityChanges = await _identityContext.SaveChangesAsync();

            return businessChanges + identityChanges;
        }
    }
}
