using StudentResult.Domain.Base;

namespace StudentResult.Domain.Entities;

public class Comment : BaseEntity<int>
{
    public int StudentAssignmentId { get; set; }

    public int CourseUserId { get; set; }

    public DateTime CommentDate { get; set; }

    public string? CommentText { get; set; }

    public virtual CourseUser CourseUser { get; set; } = null!;

    public virtual StudentAssignment Studentassignment { get; set; } = null!;
}
