using StudentResult.Application.Abstractions;
using StudentResult.Domain.Entities;
using StudentResult.Application.Abstractions.Errors;
using StudentResult.Application.DTOs;
using FileInfo = StudentResult.Application.DTOs.FileInfo;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Infrastructure.AWS;

namespace StudentResult.Application.Queries.GetAllStudentAssignment {
    public class GetAllStudentAssignmentQueryHandler : IQueryHandler<GetAllStudentAssignmentQuery, object> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetAllStudentAssignmentQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<object>> Handle(GetAllStudentAssignmentQuery request, CancellationToken cancellationToken) {
            Assignment assignment = await _unitOfWork.GetRepository<Assignment>().FirstOrDefaultAsync(a => a.AssignmentId == request.AssignmentId);
            if (assignment == null) return Errors.Global.Error404();
            CourseUser teacher = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.UserId == request.UserId && cu.CourseId == assignment.CourseId && cu.Role != 2);
            if (teacher == null) return Errors.Global.Error403();
            
            List<StudentAssignment> studentAssignments =  _unitOfWork.GetRepository<StudentAssignment>().FindBy(sa => sa.AssignmentId == request.AssignmentId).ToList();
            // провірка списку

            List<SAInfo> res = new List<SAInfo>();
            foreach (StudentAssignment studentAssignment in studentAssignments) {
                CourseUser student = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == studentAssignment.StudentId);
                if (student == null) continue;

                string StudentImageLink = String.Empty;
                if (student.User.UserImage != null) {
                    try {
                        StudentImageLink = await _s3.GetObjectTemporaryUrlAsync(student.User.UserImage);
                    }
                    catch { }
                }

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


                res.Add(new SAInfo(studentAssignment,
                    comments_info,
                    studentAssignment.AttachedFiles.ToList(),
                    new UserInfo(student.User, StudentImageLink)));
            }

            return res;
        }
    }
}
