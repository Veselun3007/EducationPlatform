using CourseService.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities;
public partial class User : IAggregateRoot {
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string? UserImage { get; set; }

    [JsonIgnore]
    public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
}
