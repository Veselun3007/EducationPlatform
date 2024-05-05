using StudentResult.Domain.Entities;

namespace StudentResult.Application.DTOs {
    public class SAInfo {
        public SAInfo() {}

        public SAInfo(StudentAssignment studentAssignment, List<CommentInfo>? comments, List<AttachedFile>? files, UserInfo? userInfo) {
            StudentAssignment = studentAssignment;
            Comments = comments;
            Files = files;
            UserInfo = userInfo;
        }
        public SAInfo(StudentAssignment studentAssignment, List<CommentInfo>? comments, List<AttachedFile>? files) {
            StudentAssignment = studentAssignment;
            Comments = comments;
            Files = files;
        }

        public StudentAssignment StudentAssignment { get; set; }
        public List<CommentInfo>? Comments { get; set; }
        public List<AttachedFile>? Files { get; set; }
        //public List<FileInfo>? Files { get; set; }
        public UserInfo? UserInfo { get; set; }
    }
}
