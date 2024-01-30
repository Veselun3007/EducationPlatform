using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class MessageReader : IEntity
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public bool IsRead { get; set; } = false;

        public int MemberId { get; set; }
    }
}