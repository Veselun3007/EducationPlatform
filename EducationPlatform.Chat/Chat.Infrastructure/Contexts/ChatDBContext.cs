using EPChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Infrastructure.Contexts
{
    public class ChatDBContext : DbContext
    {
        public ChatDBContext() { }

        public ChatDBContext(DbContextOptions<ChatDBContext> options)
            : base(options) { }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatMember> ChatMembers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageMedia> MessagesMedia { get; set; }

        public DbSet<MessageReader> MessageReaders { get; set; }
    }
}
