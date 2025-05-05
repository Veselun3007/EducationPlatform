using CourseContent.Domain.Base;

namespace CourseContent.Domain.Entities
{
    public class Materialfile : BaseEntity<int>
    {
        public int MaterialId { get; set; }

        public string? MaterialFile { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}