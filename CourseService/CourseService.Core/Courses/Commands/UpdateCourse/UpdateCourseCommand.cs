using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;

namespace CourseService.Application.Courses.Commands.UpdateCourse {
    public class UpdateCourseCommand : ICommand<Course> {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }
    }
}
