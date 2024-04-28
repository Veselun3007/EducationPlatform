using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateTeacher {
    public class CreateAdminCommand : IRequest<Result<NewCourseInfo>> {
        public CreateAdminCommand(NewCourseInfo newCourseInfo, string userId) {
            NewCourseInfo = newCourseInfo;
            UserId = userId;
        }

        public NewCourseInfo NewCourseInfo { get; set; }
        public string UserId { get; set; }
    }
}
