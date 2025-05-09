using StudentResult.Domain.Base;

namespace StudentResult.Domain.Entities;

public class AttachedFile : BaseEntity<int>
{
    public int StudentassignmentId { get; set; }

    public required string AttachedFileName { get; set; }

    public virtual StudentAssignment Studentassignment { get; set; } = null!;
}
