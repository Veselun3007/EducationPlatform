using EPChat.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EPChat.Domain.Entities
{
    public  class Message : IEntity
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int? ReplyToMessageId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.Now;

        public bool HasAttachment { get; set; } = false;

        public bool IsEdit { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<MessageMedia> AttachedMedias { get; set; } = new List<MessageMedia>();

        public virtual ICollection<MessageReader> Readers { get; set; } = new List<MessageReader>();

        public virtual Message? ReplyToMessage { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<IFormFile>? AttachedFiles { get; set; }
    }
}
