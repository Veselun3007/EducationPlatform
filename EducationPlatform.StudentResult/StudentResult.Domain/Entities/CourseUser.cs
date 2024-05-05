using System.Text.Json.Serialization;

namespace StudentResult.Domain.Entities;

public partial class CourseUser {
    public int CourseUserId { get; set; }

    public int CourseId { get; set; }

    public string UserId { get; set; } = null!;

    public int Role { get; set; }

    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
