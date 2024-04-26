using CourseService.Application.Abstractions;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.DeleteCourseuser {
    public class DeleteCourseuserCommand : IRequest<Result<Object>> {
        public DeleteCourseuserCommand(int courseuserId) {
            CourseuserId = courseuserId;
        }
        public int CourseuserId { get; set; }
    }
}
