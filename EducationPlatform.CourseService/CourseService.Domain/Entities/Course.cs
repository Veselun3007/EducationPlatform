using CourseService.Domain.Base;

namespace CourseService.Domain.Entities
{
    public class Course : AggregateRoot<int>
    {
        public string CourseName { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public string? CourseLink { get; set; }

        public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
    }
}