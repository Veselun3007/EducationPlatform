using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities
{
    public class Materiallink : IAggregateRoot<int>
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public string? MaterialLink { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}