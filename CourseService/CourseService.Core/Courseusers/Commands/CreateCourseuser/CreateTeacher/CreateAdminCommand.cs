using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateTeacher {
    public class CreateAdminCommand : IRequest<Result<Courseuser>> {
        public CreateAdminCommand(Course course, string userId, int role, bool isAdmin) {
            Course = course;
            UserId = userId;
            Role = role;
            IsAdmin = isAdmin;
        }

        public Course Course { get; set; }
        public string UserId { get; set; }
        public int Role { get; set; }
        public bool IsAdmin { get; set; }
    }
}
