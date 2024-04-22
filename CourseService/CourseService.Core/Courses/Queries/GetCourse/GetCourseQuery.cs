using CourseService.Application.Abstractions;

namespace CourseService.Application.DTOs.Courses.Queries {
    public class GetCourseQuery : IQuery<CourseInfo> {
        public GetCourseQuery(int courseId, string userId) {
            CourseId = courseId;
            UserId = userId;
        }
        public int CourseId { get; set; }
        public string UserId { get; set; }
    }
}
