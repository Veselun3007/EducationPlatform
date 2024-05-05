using StudentResult.Domain.Entities;

namespace StudentResult.Application.DTOs {
    public class CommentInfo {
        public CommentInfo() {}

        public CommentInfo(Comment comment, UserInfo userInfo) {
            Comment = comment;
            UserInfo = userInfo;
        }
        public CommentInfo(Comment comment, User user) {
            Comment = comment;
            UserInfo = new UserInfo(user);
        }
        public CommentInfo(Comment comment, User user, string imageLink) {
            Comment = comment;
            UserInfo = new UserInfo(user, imageLink);
        }

        public Comment Comment { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
