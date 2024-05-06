using System.Text.Json.Serialization;

namespace StudentResult.Domain.Entities;

public partial class StudentAssignment {
    public int StudentassignmentId { get; set; }

    public int AssignmentId { get; set; }

    public int StudentId { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public int? CurrentMark { get; set; }

    public bool? IsDone { get; set; }

    [JsonIgnore]
    public virtual Assignment Assignment { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<AttachedFile> AttachedFiles { get; set; } = new List<AttachedFile>();

    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual CourseUser Student { get; set; } = null!;
}
