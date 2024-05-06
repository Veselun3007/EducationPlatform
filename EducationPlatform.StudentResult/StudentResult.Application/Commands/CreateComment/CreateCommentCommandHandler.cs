using MediatR;
using StudentResult.Application.Abstractions;
using StudentResult.Application.DTOs;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.AWS;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Application.Abstractions.Errors;

namespace StudentResult.Application.Commands.CreateComment {
    public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, object> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<object>> Handle(CreateCommentCommand request, CancellationToken cancellationToken) {
            StudentAssignment studentAssignment = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.StudentassignmentId == request.StudentAssignmentId);
            if (studentAssignment == null) return Errors.Global.Error404();
            CourseUser student = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == studentAssignment.StudentId);
            if (student == null) return Errors.Global.Error404();
            CourseUser sender = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.UserId == request.UserId && cu.CourseId == student.CourseId && (cu.UserId == student.UserId || cu.Role != 2));
            if (sender == null) return Errors.Global.Error403();

            await _unitOfWork.GetRepository<Comment>().AddAsync(new Comment() {
                CourseUser = sender,
                CommentDate = DateTime.UtcNow,
                CommentText = request.CommentText,
                Studentassignment = studentAssignment,
            });

            //var comments = _unitOfWork.GetRepository<Comment>().FindBy(c => c.StudentassignmentId == request.StudentAssignmentId).ToList();
            var comments = studentAssignment.Comments;
            List<CommentInfo> comments_info = new List<CommentInfo>();
            foreach (Comment comment in comments) {
                CourseUser courseUser = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == comment.CourseUserId);
                if (courseUser == null) continue;
                User user = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(u => u.UserId == courseUser.UserId);
                if (user == null) continue;
                string ImageLink = String.Empty;
                if (user.UserImage != null) {
                    try {
                        ImageLink = await _s3.GetObjectTemporaryUrlAsync(user.UserImage);
                    }
                    catch { }
                }
                comments_info.Add(new CommentInfo(comment, user, ImageLink));
            }
            comments_info.OrderBy(c => c.Comment.CommentDate);

            return comments_info;
        }
    }
}
