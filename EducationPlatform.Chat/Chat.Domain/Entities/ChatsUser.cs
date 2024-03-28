using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class ChatsUser : IEntity
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public virtual ICollection<ChatMember> Memberships { get; set; } = new List<ChatMember>();
    }
}
