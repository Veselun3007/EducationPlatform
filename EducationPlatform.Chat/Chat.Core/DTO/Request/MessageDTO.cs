using EPChat.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EPChat.Core.DTO.Request
{
    public class MessageDTO
    {
        public int ChatId { get; set; }

        public int? ReplyToMessageId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.Now;

        public bool? HasAttachment { get; set; } 

        public virtual Message? ReplyToMessage { get; set; }

        public List<IFormFile>? AttachedFiles { get; set; }
    }
}
