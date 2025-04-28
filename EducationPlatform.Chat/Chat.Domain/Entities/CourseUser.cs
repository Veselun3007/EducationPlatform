using Chat.Domain.Entities.Base;

namespace Chat.Domain.Entities
{
    public class CourseUser : BaseEntity<int>
    {
        public int CourseId { get; set; }

        public string UserId { get; set; } = null!;

        public int Role { get; set; }

        public virtual Course? Course { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; } = [];
    }
}
