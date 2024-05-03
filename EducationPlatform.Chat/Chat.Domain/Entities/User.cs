using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }

        public virtual ICollection<ChatMember> Members { get; set; } = [];
    }
}
