using CourseService.Application.Abstractions;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateStudent {
    public class CreateStudentCommand : IRequest<Result> {
        public CreateStudentCommand(string courseLink, string userId) {
            CourseLink = courseLink;
            UserId = userId;
        }
        public CreateStudentCommand() { }

        public string CourseLink { get; set; }
        public string UserId { get; set; }
    }
}
