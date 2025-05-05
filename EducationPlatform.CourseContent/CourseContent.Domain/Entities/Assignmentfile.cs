using CourseContent.Domain.Base;

namespace CourseContent.Domain.Entities
{
    public class Assignmentfile : BaseEntity<int>
    {
        public int AssignmentId { get; set; }

        public string? AssignmentFile { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;
    }
}