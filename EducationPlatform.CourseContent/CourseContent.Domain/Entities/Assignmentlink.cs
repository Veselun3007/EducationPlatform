using CourseContent.Domain.Base;

namespace CourseContent.Domain.Entities
{
    public class Assignmentlink : BaseEntity<int>
    {
        public int AssignmentId { get; set; }

        public string? AssignmentLink { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;
    }
}