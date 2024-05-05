using StudentResult.Application.Abstractions;
using StudentResult.Domain.Entities;
using StudentResult.Application.Abstractions.Errors;
using StudentResult.Application.DTOs;
using FileInfo = StudentResult.Application.DTOs.FileInfo;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Infrastructure.AWS;

namespace StudentResult.Application.Queries.GetStudentAssignment {
    public class GetStudentAssignmentQueryHandler : IQueryHandler<GetStudentAssignmentQuery, object> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetStudentAssignmentQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<object>> Handle(GetStudentAssignmentQuery request, CancellationToken cancellationToken) {
            Assignment assignment = await _unitOfWork.GetRepository<Assignment>().FirstOrDefaultAsync(a => a.AssignmentId == request.AssignmentId);
            if (assignment == null) return Errors.Global.Error404();
            CourseUser courseUser = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.UserId == request.UserId && cu.CourseId == assignment.CourseId);
            if (courseUser == null) return Errors.Global.Error403();
            StudentAssignment studentAssignment = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.AssignmentId == request.AssignmentId && sa.StudentId == courseUser.CourseUserId);
            if (studentAssignment == null) return Errors.Global.Error404();

            //string ImageLink = String.Empty;
            //if (courseUser.User.UserImage != null) {
            //    try {
            //        ImageLink = await _s3.GetObjectTemporaryUrlAsync(courseUser.User.UserImage);
            //    }
            //    catch {}
            //}

            List<CommentInfo> comments_info = new List<CommentInfo>();
            foreach (Comment comment in studentAssignment.Comments) {
                CourseUser sender = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == comment.CourseUserId);
                if (sender == null) continue;
                User user = sender.User;
                string SenderImageLink = String.Empty;
                if (user.UserImage != null) {
                    try {
                        SenderImageLink = await _s3.GetObjectTemporaryUrlAsync(user.UserImage);
                    }
                    catch { }
                }
                comments_info.Add(new CommentInfo(comment, user, SenderImageLink));
            }
            comments_info.OrderBy(c => c.Comment.CommentDate);

            return new SAInfo(studentAssignment,
                comments_info,
                studentAssignment.AttachedFiles.ToList());
        }
    }
}
