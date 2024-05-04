using EPChat.Domain.Entities;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using EPChat.Infrastructure.Repositories;

namespace EPChat.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _context;

        public UnitOfWork(EducationPlatformContext context)
        {
            _context = context;

            MessageRepository = new Repository<Message>(_context);
            MessageMediaRepository = new RepositoryMin<MessageMedia>(_context);
            MemberRepository = new GetRepository<CourseUser>(_context);
        }

        public IRepository<Message> MessageRepository { get; private set; }
        public IMinRepository<MessageMedia> MessageMediaRepository { get; private set; }
        public IGetRepository<CourseUser> MemberRepository { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
