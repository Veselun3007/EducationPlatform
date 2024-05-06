using EPChat.Domain.Entities;

namespace EPChat.Core.DTO.Request
{
    public class MessageUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public static Message FromMessageUpdateDTO(MessageUpdateDTO messageDTO)
        {
            return new Message
            {
                Id = messageDTO.Id,
                CourseId = messageDTO.CourseId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
            };
        }
    }
}
