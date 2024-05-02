using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class Chat : IEntity
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public virtual ICollection<ChatMember> Members { get; set; } = new List<ChatMember>();

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}