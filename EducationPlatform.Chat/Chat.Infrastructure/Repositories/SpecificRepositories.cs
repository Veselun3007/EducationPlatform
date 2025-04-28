using Chat.Domain.Entities;
using Chat.Infrastructure.Contexts;

namespace Chat.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message, int>
    {
        public MessageRepository(EducationPlatformContext context) : base(context) { }
    }

    public class MessageMediaRepository : RepositoryMin<MessageMedia, int>
    {
        public MessageMediaRepository(EducationPlatformContext context) : base(context) { }
    }
}
