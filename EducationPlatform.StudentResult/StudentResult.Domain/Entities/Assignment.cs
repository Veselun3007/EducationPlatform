namespace StudentResult.Domain.Entities;

public partial class Assignment {
    public int AssignmentId { get; set; }

    public int CourseId { get; set; }

    public int? TopicId { get; set; }

    public string AssignmentName { get; set; } = null!;

    public string? AssignmentDescription { get; set; }

    public DateTime AssignmentDatePublication { get; set; }

    public DateTime AssignmentDeadline { get; set; }

    public int MaxMark { get; set; }

    public int MinMark { get; set; }

    public bool IsRequired { get; set; }

    public bool? IsEdited { get; set; }

    public DateTime? EditedTime { get; set; }

    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();
}
