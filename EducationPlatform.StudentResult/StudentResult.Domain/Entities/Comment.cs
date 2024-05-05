using System.Text.Json.Serialization;

namespace StudentResult.Domain.Entities;

public partial class Comment {
    public int CommentId { get; set; }

    public int StudentassignmentId { get; set; }

    public int CourseUserId { get; set; }

    public DateTime CommentDate { get; set; }

    public string? CommentText { get; set; }

    [JsonIgnore]
    public virtual CourseUser CourseUser { get; set; } = null!;

    [JsonIgnore]
    public virtual StudentAssignment Studentassignment { get; set; } = null!;
}
