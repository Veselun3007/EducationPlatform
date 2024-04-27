using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;

namespace CourseService.Application.Courses.Commands.UpdateCourse {
    public class UpdateCourseCommand : ICommand<CourseInfo> {
        public UpdateCourseCommand() {}

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public string UserId { get; set; }
    }
}
