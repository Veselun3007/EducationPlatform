using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;

namespace CourseService.Application.Courses.Commands.CreateCourse {
    public class CreateCourseCommand : ICommand<Course> {
        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }
    }
}
