using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;

namespace CourseService.Application.Courses.Queries.GetAllCourses {
    public class GetAllCoursesQuery : IQuery<IEnumerable<CourseInfo>> {
        public GetAllCoursesQuery(string userId) {
            UserId = userId;
        }
        public string UserId { get; set; }
    }
}
