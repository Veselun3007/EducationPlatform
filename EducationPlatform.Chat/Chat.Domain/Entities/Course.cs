using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities
{
    public class Course : BaseEntity<int>
    {
        public string CourseName { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public string? CourseLink { get; set; }

        public virtual ICollection<CourseUser> CourseUsers { get; set; } = [];

        public virtual ICollection<Message> Messages { get; set; } = [];
    }
}