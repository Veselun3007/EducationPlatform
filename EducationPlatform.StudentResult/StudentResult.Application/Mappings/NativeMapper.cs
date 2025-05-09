using StudentResult.Domain.Entities;
using StudentResult.Application.DTO.Request;
using StudentResult.Application.DTO.Response;

namespace StudentResult.Application.Mappings
{
    internal static class NativeMapper
    {
        public static Comment ToComment(CommentDto commentDto)
        {
            return new Comment()
            {
                CommentText = commentDto.CommentText,
                CommentDate = commentDto.CommentDate,
                StudentAssignmentId = commentDto.StudentAssignmentId,
                CourseUserId = commentDto.CourseUserId,
            };
        }

        public static FileInfoDto FromAttachedFile(AttachedFile file)
        {
            return new FileInfoDto
            {
                FileId = file.Id,
                FileName = file.AttachedFileName
            };
        }

        public static List<FileInfoDto> FromAttachedFile(IEnumerable<AttachedFile> files)
        {
            return files.Select(FromAttachedFile).ToList();
        }

        public static CommentInfoDto ToCommentInfo(Comment comment, User user, string? imageLink = null)
        {
            return new CommentInfoDto
            {
                CommentId = comment.Id,
                CourseUserId = comment.CourseUserId,
                CommentDate = comment.CommentDate,
                CommentText = comment.CommentText,
                UserId = user.Id,
                UserName = user.UserName,
                ImageLink = imageLink
            };
        }

        public static StudentAssignmentInfoDto ToStudentAssignmentInfo(StudentAssignment studentAssignment, List<CommentInfoDto> comments, List<FileInfoDto> files)
        {
            return new StudentAssignmentInfoDto
            {
                AssignmentId = studentAssignment.AssignmentId,
                StudentId = studentAssignment.StudentId,
                SubmissionDate = studentAssignment.SubmissionDate,
                CurrentMark = studentAssignment.CurrentMark,
                IsDone = studentAssignment.IsDone,
                Comments = comments,
                Files = files
            };
        }

        public static StudentAssignment ToStudentAssignment(UpdateStudentAssignmentDto dto)
        {
            return new StudentAssignment
            {
                Id = dto.StudentAssignmentId,
                AssignmentId = dto.AssignmentId,
                StudentId = dto.StudentId,
                SubmissionDate = dto.SubmissionDate,
                CurrentMark = dto.CurrentMark,
                IsDone = dto.IsDone
            };
        }

        public static AttachedFile ToAttachedFile(string link, int id)
        {
            return new AttachedFile
            {
                AttachedFileName = link,
                StudentassignmentId = id
            };
        }
    }
}
