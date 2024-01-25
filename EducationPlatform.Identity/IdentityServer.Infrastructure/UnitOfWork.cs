using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Repositories;

namespace IdentityServer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly EducationPlatformContext _context;

        public UnitOfWork(EducationPlatformContext context)
        {
            _context = context;

            UserRepository = new Repository<User>(_context);
        }
        public IRepository<User> UserRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing)
            {
                return;
            }

            _context.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
