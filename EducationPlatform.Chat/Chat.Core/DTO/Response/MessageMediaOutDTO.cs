using Chat.Domain.Entities;

namespace Chat.Core.DTO.Response
{
    public class MessageMediaOutDTO
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string? MediaLink { get; set; }
    }
}
