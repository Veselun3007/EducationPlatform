using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.UpdateCourseuser {
    public class UpdateCourseuserCommand : IRequest<Result<CourseUserInfo>> {
        public string UserId { get; set; }

        public int CourseuserId { get; set; }
        public int Role { get; set; }
    }
}
