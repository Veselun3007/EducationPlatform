using EPChat.Domain.Entities;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using EPChat.Infrastructure.Repositories;

namespace EPChat.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDBContext _context;

        public UnitOfWork(ChatDBContext context)
        {
            _context = context;

            MessageRepository = new Repository<Message>(_context);
            MessageMediaRepository = new RepositoryMin<MessageMedia>(_context);
        }

        public IRepository<Message> MessageRepository { get; private set; }

        public IMinRepository<MessageMedia> MessageMediaRepository { get; private set; }     

        public async Task<int> ComplectAsync()
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
