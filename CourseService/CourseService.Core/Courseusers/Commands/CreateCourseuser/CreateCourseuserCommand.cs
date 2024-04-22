using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser {
    public class CreateCourseuserCommand : IRequest<Result<Courseuser>> {
        public CreateCourseuserCommand(string courseLink, string userId) {
            CourseLink = courseLink;
            UserId = userId;
        }
        public CreateCourseuserCommand(Course courseLink, string userId, int role, bool isAdmin) {
            Course = courseLink;
            UserId = userId;
            Role = role;
            IsAdmin = isAdmin;
        }

        //видалити
        public Course Course { get; set; }

        public string CourseLink { get; set; }
        public string UserId { get; set; }
        public int Role { get; set; }
        public bool IsAdmin { get; set; }
    }
}
