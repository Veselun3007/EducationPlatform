using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class Course : IEntity
    {
        public int Id { get; set; }

        public string CourseName { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public string? CourseLink { get; set; }

        public int? ChatId { get; set; }

        public virtual ICollection<ChatMember> Members { get; set; } = [];

        public virtual ICollection<Message> Messages { get; set; } = [];

    }
}