using Chat.Core.Interfaces.Infrastructure;
using Chat.Domain.Entities;
using Chat.Infrastructure.Contexts;
using Chat.Infrastructure.Repositories;

namespace Chat.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _context;

        public UnitOfWork(EducationPlatformContext context)
        {
            _context = context;

            MessageRepository = new MessageRepository(_context);
            MessageMediaRepository = new MessageMediaRepository(_context);
        }

        public IRepository<Message, int> MessageRepository { get; private set; }
        public IMinRepository<MessageMedia, int> MessageMediaRepository { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
