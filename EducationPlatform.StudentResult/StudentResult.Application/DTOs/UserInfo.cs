using StudentResult.Domain.Entities;

namespace StudentResult.Application.DTOs {
    public class UserInfo {
        public UserInfo() {}
        public UserInfo(User user) {
            UserId = user.UserId; 
            UserName = user.UserName;
            ImageLink = user.UserImage;
        }
        public UserInfo(User user, string imageLink) {
            UserId = user.UserId;
            UserName = user.UserName;
            ImageLink = imageLink;
        }
        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ImageLink { get; set; }
    }
}
