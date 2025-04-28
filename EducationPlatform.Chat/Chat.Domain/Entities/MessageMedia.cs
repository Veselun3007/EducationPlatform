using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities
{
    public class MessageMedia : BaseEntity<int>
    {
        public int MessageId { get; set; }

        public string? MediaLink { get; set; }

        public virtual Message? Message { get; set; }
    }
}
