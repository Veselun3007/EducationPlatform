using CourseService.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities;
public partial class Courseuser : IAggregateRoot {
    public Courseuser(int courseuserId, int courseId, string userId, bool isAdmin, int role, Course course, User user) {
        CourseuserId = courseuserId;
        CourseId = courseId;
        UserId = userId;
        IsAdmin = isAdmin;
        Role = role;
        Course = course;
        User = user;
    }
    public Courseuser(int courseId, string userId, bool isAdmin, int role) {
        CourseId = courseId;
        UserId = userId;
        IsAdmin = isAdmin;
        Role = role;
    }
    public Courseuser() { }

    public int CourseuserId { get; set; }

    public int CourseId { get; set; }

    public string UserId { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public int Role { get; set; }

    [JsonIgnore]
    public virtual Course Course { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
