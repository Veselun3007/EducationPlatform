using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities
{
    public class Topic : IAggregateRoot
    {
        public int Id { get ; set; }

        public int CourseId { get ; set; }

        public required string Title { get; set; }

        public virtual Material? Material { get; set; }

        public virtual ICollection<Material>? Materials { get; set; } = new List<Material>();

        public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();
    }
}
