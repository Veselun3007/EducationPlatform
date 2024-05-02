using EPChat.Domain.Entities;

namespace EPChat.Core.DTO.Response
{
    public class MessageOutDTO
    {
        public int ChatId { get; set; }

        public int? ReplyToMessageId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; }

        public bool? IsEdit { get; set; }

        public DateTime? EditedIn { get; set; }

        public ICollection<MessageMediaOutDTO>? AttachedFiles { get; set; }

        public static MessageOutDTO FromMessage(Message message)
        {
            return new MessageOutDTO
            {
                ChatId = message.ChatId,
                ReplyToMessageId = message.ReplyToMessageId,
                MessageText = message.MessageText,
                CreatorId = message.CreatorId,
                CreatedIn = message.CreatedIn,
                IsEdit = message.IsEdit,
                EditedIn = message.EditedIn,
                AttachedFiles = message
                    .AttachedMedias.Select(m => MessageMediaOutDTO
                    .FromMessageMedia(m)).ToList(),
            };
        }
    }
}
