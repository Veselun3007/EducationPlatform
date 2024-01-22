using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Repositories;

namespace IdentityServer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly EducationPlatformContext _context;
        private readonly FileHelper _filesHelper;

        public UnitOfWork(EducationPlatformContext context, FileHelper fileHelper)
        {
            _context = context;
            _filesHelper = fileHelper;

            UserRepository = new Repository<User>(_context, _filesHelper);
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
