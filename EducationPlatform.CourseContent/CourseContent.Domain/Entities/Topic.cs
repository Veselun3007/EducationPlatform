using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities
{
    public class Topic : IAggregateRoot
    {
        public int Id { get; set; }

        public required int CourseId { get; set; }

        public required string Title { get; set; }

        public virtual Course Course { get; set; } = null!;

        public virtual ICollection<Material>? Materials { get; set; } = [];

        public virtual ICollection<Assignment>? Assignments { get; set; } = [];
    }
}
