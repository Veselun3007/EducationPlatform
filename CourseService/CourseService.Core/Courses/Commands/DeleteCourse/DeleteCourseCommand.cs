using CourseService.Application.Abstractions;

namespace CourseService.Application.Courses.Commands.DeleteCourse {
    public class DeleteCourseCommand : ICommand {
        public DeleteCourseCommand(int courseId) {
            CourseId = courseId;
        }
        public int CourseId { get; set; }
    }
}
