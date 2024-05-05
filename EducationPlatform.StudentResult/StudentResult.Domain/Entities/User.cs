using System.Text.Json.Serialization;

namespace StudentResult.Domain.Entities;

public partial class User {
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string? UserImage { get; set; }

    [JsonIgnore]
    public virtual ICollection<CourseUser> CourseUsers { get; set; } = new List<CourseUser>();
}
