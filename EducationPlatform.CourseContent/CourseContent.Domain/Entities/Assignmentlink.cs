using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Assignmentlink : IAggregateRoot
{
    public int Id { get; set; }

    public int AssignmentId { get; set; }

    public string? AssignmentLink { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;
}
