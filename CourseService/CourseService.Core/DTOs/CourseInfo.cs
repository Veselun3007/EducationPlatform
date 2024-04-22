using CourseService.Domain.Entities;

namespace CourseService.Application.DTOs {
    public class CourseInfo {
        public Course Course { get; set; }
        public UserInfo UserInfo { get; set; }
        public AdminInfo AdminInfo { get; set; }
    }
    public class UserInfo {
        public int Role { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class AdminInfo {
        public string? ImageLink { get; set; }
        public string AdminName { get; set; }
    }
}
