using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateStudent {
    public class CreateStudentCommand : IRequest<Result<Courseuser>> {
        public CreateStudentCommand(string courseLink, string userId) {
            CourseLink = courseLink;
            UserId = userId;
        }
        public CreateStudentCommand(string courseLink, string userId, int role, bool isAdmin) {
            CourseLink = courseLink;
            UserId = userId;
            Role = role;
            IsAdmin = isAdmin;
        }

        public CreateStudentCommand() { }

        public string CourseLink { get; set; }
        public string UserId { get; set; }
        public int Role { get; set; }
        public bool IsAdmin { get; set; }
    }
}
