using CourseService.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities;
public partial class Course : IAggregateRoot {
    public Course(int courseId, string courseName, string? courseDescription, string? courseLink) {
        CourseId = courseId;
        CourseName = courseName;
        CourseDescription = courseDescription;
        CourseLink = courseLink;
    }
    public Course(string courseName, string? courseDescription, string? courseLink) {
        CourseName = courseName;
        CourseDescription = courseDescription;
        CourseLink = courseLink;
    }

    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public string? CourseDescription { get; set; }

    public string? CourseLink { get; set; }

    [JsonIgnore]
    public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
}
