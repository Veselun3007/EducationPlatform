using CourseContent.Domain.Base;

namespace CourseContent.Domain.Entities
{
    public class Materiallink : AggregateRoot<int>
    {
        public int MaterialId { get; set; }

        public string? MaterialLink { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}