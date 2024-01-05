using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Assignmentfile : IAggregateRoot
{
    public int AssignmentAttachedfileId { get; set; }

    public int AssignmentId { get; set; }

    public string? AssignmentFile { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;
}
