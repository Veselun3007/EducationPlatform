namespace CourseService.Application.DTOs {
    public class CourseUserInfo {
        public int CourseuserId { get; set; }

        public bool IsAdmin { get; set; }

        public int Role { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }
    }
}
