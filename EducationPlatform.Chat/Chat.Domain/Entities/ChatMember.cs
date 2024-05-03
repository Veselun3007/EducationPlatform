using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class ChatMember : IEntity
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int UserId { get; set; }

        public virtual Course? Course { get; set; }

        public virtual User? ChatsUser { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = [];

    }
}
