using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class Message : IEntity
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; }

        public bool IsEdit { get; set; } = false;

        public DateTime? EditedIn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<MessageMedia> AttachedMedias { get; set; } = [];

        public virtual Course? Course { get; set; }

        public virtual CourseUser? CourseUser { get; set; }
    }
}
