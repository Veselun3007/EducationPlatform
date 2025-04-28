using Chat.Domain.Entities;

namespace Chat.Core.DTO.Request
{
    public class MessageDTO
    {
        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.UtcNow;

        public List<MessageMediaDTO>? AttachedFiles { get; set; }
    }
}
