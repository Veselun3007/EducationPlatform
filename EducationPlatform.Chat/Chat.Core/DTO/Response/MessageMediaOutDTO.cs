using EPChat.Domain.Entities;

namespace EPChat.Core.DTO.Response
{
    public class MessageMediaOutDTO
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string? MediaLink { get; set; }

        public static MessageMediaOutDTO FromMessageMedia(MessageMedia media)
        {
            return new MessageMediaOutDTO
            {
                Id = media.Id,
                MessageId = media.MessageId,
                MediaLink = media.MediaLink
            };
        }
    }
}
