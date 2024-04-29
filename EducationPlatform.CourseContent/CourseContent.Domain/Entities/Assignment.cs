using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Assignment : IAggregateRoot
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int? TopicId { get; set; }

    public required string AssignmentName { get; set; }

    public string? AssignmentDescription { get; set; }

    public required DateTime AssignmentDatePublication { get; set; }

    public required DateTime AssignmentDeadline { get; set; }

    public int MaxMark { get; set; }

    public int MinMark { get; set; }

    public bool IsRequired { get; set; }

    public bool IsEdited { get; set; } = false;

    public DateTime EditedTime { get; set; }

    public virtual ICollection<Assignmentfile> Assignmentfiles { get; set; } = [];

    public virtual ICollection<Assignmentlink> Assignmentlinks { get; set; } = [];

    public virtual Course Course { get; set; } = null!;

    public virtual Topic? Topic { get; set; }
}
