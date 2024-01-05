using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Assignment : IAggregateRoot
{
    public int AssignmentId { get; set; }

    public int CourseId { get; set; }

    public string AssignmentName { get; set; } = null!;

    public string? AssignmentDescription { get; set; }

    public DateTime AssignmentDatePublication { get; set; } = DateTime.Now;

    public DateTime AssignmentDeadline { get; set; }

    public virtual ICollection<Assignmentfile>? Assignmentfiles { get; set; } = new List<Assignmentfile>();

    public virtual Course? Course { get; set; } = null!;
   
}
