using StudentResult.Domain.Base;

namespace StudentResult.Domain.Entities;

public class StudentAssignment : BaseEntity<int>
{
    public int AssignmentId { get; set; }

    public int StudentId { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public int? CurrentMark { get; set; }

    public bool? IsDone { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual ICollection<AttachedFile> AttachedFiles { get; set; } = new List<AttachedFile>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual CourseUser Student { get; set; } = null!;
}
