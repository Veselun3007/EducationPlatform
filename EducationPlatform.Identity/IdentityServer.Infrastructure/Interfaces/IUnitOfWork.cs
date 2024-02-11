using IdentityServer.Domain.Entities;

namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }

        IRepository<AppUser> IdentityRepository { get; }

        IBaseRepository<Token> TokenRepository { get; }

        Task<int> CompleteAsync();
    }
}
