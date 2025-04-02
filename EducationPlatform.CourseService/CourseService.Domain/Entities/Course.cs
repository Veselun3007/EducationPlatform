using CourseService.Domain.Base;
using System.Text.Json.Serialization;

namespace CourseService.Domain.Entities
{
    public class Course : AggregateRoot<int>
    {
        public Course(int courseId, string courseName, string? courseDescription, string? courseLink)
        {
            Id = courseId;
            CourseName = courseName;
            CourseDescription = courseDescription;
            CourseLink = courseLink;
        }
        public Course(string courseName, string? courseDescription, string? courseLink)
        {
            CourseName = courseName;
            CourseDescription = courseDescription;
            CourseLink = courseLink;
        }

        public string CourseName { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public string? CourseLink { get; set; }

        [JsonIgnore]
        public virtual ICollection<Courseuser> Courseusers { get; set; } = new List<Courseuser>();
    }
}