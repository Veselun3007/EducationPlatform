using EPChat.Core.Models.HelperModel;
using EPChat.Domain.Entities;

namespace EPChat.Core.DTO.Request
{
    public class MessageDTO
    {
        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.UtcNow;

        public List<MediaMessage>? AttachedFiles { get; set; }

        public static Message FromMessageDTO(MessageDTO messageDTO)
        {
            return new Message
            {
                CourseId = messageDTO.CourseId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
                CreatedIn = messageDTO.CreatedIn,
            };
        }
    }
}
