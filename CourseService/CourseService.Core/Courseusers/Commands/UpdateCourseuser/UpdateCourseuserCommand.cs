using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.UpdateCourseuser {
    public class UpdateCourseuserCommand : IRequest<Result<Courseuser>> {
        public int CourseuserId { get; set; }
        public bool IsAdmin { get; set; }
        public int Role { get; set; }
    }
}
