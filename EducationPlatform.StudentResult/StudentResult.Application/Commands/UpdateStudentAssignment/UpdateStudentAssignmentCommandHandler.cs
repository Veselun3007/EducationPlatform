using MediatR;
using StudentResult.Application.DTOs;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.AWS;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Application.Abstractions.Errors;
using FileInfo = StudentResult.Application.DTOs.FileInfo;
using StudentResult.Application.Abstractions;
using StudentResult.Infrastructure.Repositories;
using System.Security.AccessControl;

namespace StudentResult.Application.Commands.UpdateStudentAssignment {
    public class UpdateStudentAssignmentCommandHandler : ICommandHandler<UpdateStudentAssignmentCommand, object> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public UpdateStudentAssignmentCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<object>> Handle(UpdateStudentAssignmentCommand request, CancellationToken cancellationToken) {
            StudentAssignment studentAssignment = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.StudentassignmentId == request.StudentAssignmentId);
            if (studentAssignment == null) return Errors.Global.Error404();
            CourseUser student = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.CourseUserId == studentAssignment.StudentId && cu.UserId == request.UserId);
            if (student == null) return Errors.Global.Error403();

            // зробити в бд CurrentMark null
            if (studentAssignment.CurrentMark is not null) return Errors.Global.Error409(); // уточнити статус


            if (studentAssignment.AttachedFiles is not null | studentAssignment.AttachedFiles.Count != 0) {
                foreach (var file in studentAssignment.AttachedFiles) {
                    bool res = await _s3.DeleteObjectAsync(file.AttachedFileName);
                    if (!res) continue;
                    _unitOfWork.GetRepository<AttachedFile>().Delete(file);
                }
            }

            _unitOfWork.SaveChanges();
            //var fs = _unitOfWork.GetRepository<AttachedFile>().FindBy(f => f.StudentassignmentId == studentAssignment.StudentassignmentId).ToList();

            if (request.AssignmentFiles is not null) {
                foreach (var file in request.AssignmentFiles) {
                    string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    bool res = await _s3.PostObjectAsync(objectName, file);
                    if (!res) continue;
                    await _unitOfWork.GetRepository<AttachedFile>().AddAsync(new AttachedFile() {
                        AttachedFileName = objectName,
                        Studentassignment = studentAssignment,
                    });
                }
            }

            studentAssignment.IsDone = true;
            studentAssignment.SubmissionDate = DateTime.UtcNow;
            _unitOfWork.GetRepository<StudentAssignment>().Update(studentAssignment);

            StudentAssignment studentAssignment2 = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.StudentassignmentId == request.StudentAssignmentId);

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
                    studentAssignment.AttachedFiles.ToList());
        }
    }
}
