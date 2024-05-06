using MediatR;
using StudentResult.Application.DTOs;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.AWS;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Application.Abstractions.Errors;
using FileInfo = StudentResult.Application.DTOs.FileInfo;
using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Commands.UpdateMark {
    public class UpdateMarkCommandHandler : ICommandHandler<UpdateMarkCommand, object> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public UpdateMarkCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<object>> Handle(UpdateMarkCommand request, CancellationToken cancellationToken) {
            StudentAssignment studentAssignment = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.StudentassignmentId == request.StudentAssignmentId);
            if (studentAssignment == null) return Errors.Global.Error404();
            CourseUser student = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == studentAssignment.StudentId);
            if (student == null) return Errors.Global.Error404();
            CourseUser teacher = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.UserId == request.UserId && cu.CourseId == student.CourseId && cu.Role != 2);
            if (teacher == null) return Errors.Global.Error403();

            Assignment assignment = studentAssignment.Assignment;
            //Assignment assignment = await _unitOfWork.GetRepository<Assignment>().FirstOrDefaultAsync(a => a.AssignmentId == studentAssignment.AssignmentId);
            //if (assignment == null) return Errors.Global.Error404();



            if (request.NewMark < assignment.MinMark || request.NewMark > assignment.MaxMark) return Errors.Global.Error400();
            studentAssignment.CurrentMark = request.NewMark;
            studentAssignment.IsDone = true;
            _unitOfWork.GetRepository<StudentAssignment>().Update(studentAssignment);


            string StudentImageLink = String.Empty;
            if (student.User.UserImage != null) {
                try {
                    StudentImageLink = await _s3.GetObjectTemporaryUrlAsync(student.User.UserImage);
                }
                catch { }
            }

            //var comments = _unitOfWork.GetRepository<Comment>().FindBy(c => c.StudentassignmentId == request.StudentAssignmentId).ToList();
            List<CommentInfo> comments_info = new List<CommentInfo>();
            foreach (Comment comment in studentAssignment.Comments) {
                CourseUser sender = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == comment.CourseUserId);
                if (sender == null) continue;
                User user = sender.User;
                //User user = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(u => u.UserId == courseUser.UserId);
                //if (user == null) continue;
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
                    studentAssignment.AttachedFiles.ToList(),
                    new UserInfo(student.User, StudentImageLink));
        }
    }
}
