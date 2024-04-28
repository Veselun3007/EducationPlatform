using CourseService.Domain.Entities;

namespace CourseService.Application.DTOs {
    public class NewCourseInfo {
        public NewCourseInfo() {
            AdminInfo = new AdminInfo();
        }
        public NewCourseInfo(Course course) {
            Course = course;
            AdminInfo = new AdminInfo();
        }
        public Course Course { get; set; }
        public Courseuser UserInfo { get; set; }
        public AdminInfo AdminInfo { get; set; }
    }
}
