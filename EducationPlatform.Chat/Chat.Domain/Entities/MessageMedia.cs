using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class MessageMedia : IEntity
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string? MediaLink { get; set; }
    }
}
