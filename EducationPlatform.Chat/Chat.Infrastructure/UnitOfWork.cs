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
            MemberRepository = new GetRepository<ChatMember>(_context);
        }

        public IRepository<Message> MessageRepository { get; private set; }
        public IMinRepository<MessageMedia> MessageMediaRepository { get; private set; }
        public IGetRepository<ChatMember> MemberRepository { get; private set; }

        public async Task<int> ComplectAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
