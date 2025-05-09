using CourseService.Domain.Base;

namespace CourseService.Domain.Entities
{
    public class User : AggregateRoot<string>
    {
        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserImage { get; set; }

        public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
    }
}