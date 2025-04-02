using CourseService.Domain.Base;
using CourseService.Domain.Enums;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities
{
    public class Courseuser : AggregateRoot<int>
    {
        public int CourseId { get; set; }

        public string UserId { get; set; } = null!;

        public Roles Role { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; } = null!;

        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}