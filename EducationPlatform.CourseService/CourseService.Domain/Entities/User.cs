using CourseService.Domain.Base;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities
{
    public class User : AggregateRoot<string>
    {
        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserImage { get; set; }

        [JsonIgnore]
        public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
    }
}