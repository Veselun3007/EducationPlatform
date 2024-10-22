using CourseService.Domain.Entities;

namespace CourseService.Application.DTOs {
    public class CourseUserInfo {
        public CourseUserInfo() { }
        public CourseUserInfo(Courseuser courseuser) {
            CourseuserId = courseuser.CourseuserId;
            Role = courseuser.Role;
            UserId = courseuser.UserId;
            UserName = courseuser.User.UserName;
            UserEmail = courseuser.User.UserEmail;
            UserImage = String.Empty;
        }
        public CourseUserInfo(Courseuser courseuser, User user) {
            CourseuserId = courseuser.CourseuserId;
            Role = courseuser.Role;
            UserId = user.UserId;
            UserName = user.UserName;
            UserEmail = user.UserEmail;
            UserImage = String.Empty;
        }

        public int CourseuserId { get; set; }

        public int Role { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }
    }
}
