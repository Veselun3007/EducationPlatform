using CourseService.Domain.Base;
using CourseService.Domain.Enums;

namespace CourseService.Domain.Entities
{
    public class Courseuser : AggregateRoot<int>
    {
        public int CourseId { get; set; }

        public string UserId { get; set; } = null!;

        public Roles Role { get; set; }

        public virtual Course Course { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}