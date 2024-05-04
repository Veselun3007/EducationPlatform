using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }

        public virtual ICollection<CourseUser> CourseUsers { get; set; } = [];
    }
}
