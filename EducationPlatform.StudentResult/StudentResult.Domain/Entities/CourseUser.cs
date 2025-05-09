using StudentResult.Domain.Base;
using StudentResult.Domain.Enums;

namespace StudentResult.Domain.Entities;

public class CourseUser : BaseEntity<int>
{
    public int CourseId { get; set; }

    public string UserId { get; set; } = null!;

    public Roles Role { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();

    public virtual User User { get; set; } = null!;
}
