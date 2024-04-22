using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Courseusers.Queries.GetCourseusersByCourse {
    public class GetCourseusersByCourseQuery : IRequest<Result<IEnumerable<CourseUserInfo>>> {
        public GetCourseusersByCourseQuery(int courseId) {
            CourseId = courseId;
        }
        public int CourseId { get; set; }
    }
}
