using IdentityServer.Domain.Entities;

namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        Task<int> CompleteAsync();
    }
}
