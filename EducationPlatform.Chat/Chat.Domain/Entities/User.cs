using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities
{
    public class User : BaseEntity<string>
    {
        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }

        public virtual ICollection<CourseUser> CourseUsers { get; set; } = [];
    }
}
