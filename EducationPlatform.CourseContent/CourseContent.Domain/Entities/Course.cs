using CourseContent.Domain.Base;

namespace CourseContent.Domain.Entities
{
    public class Course : AggregateRoot<int>
    {
        public string CourseName { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public string? CourseLink { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; } = [];

        public virtual ICollection<Material> Materials { get; set; } = [];

        public virtual ICollection<Topic> Topics { get; set; } = [];
    }
}