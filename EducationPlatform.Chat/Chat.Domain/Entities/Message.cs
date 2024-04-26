using EPChat.Domain.Interfaces;

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

        public virtual ICollection<MessageMedia> AttachedMedias { get; set; } = [];

        public virtual Message? ReplyToMessage { get; set; }
    }
}
