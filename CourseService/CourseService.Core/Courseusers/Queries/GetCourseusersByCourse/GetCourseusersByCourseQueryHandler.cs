using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using MediatR;

namespace CourseService.Application.Courseusers.Queries.GetCourseusersByCourse {
    public class GetCourseusersByCourseQueryHandler : IRequestHandler<GetCourseusersByCourseQuery, Result<IEnumerable<CourseUserInfo>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetCourseusersByCourseQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }
        public async Task<Result<IEnumerable<CourseUserInfo>>> Handle(GetCourseusersByCourseQuery request, CancellationToken cancellationToken) {
            var courseusers = _unitOfWork.GetRepository<Courseuser>().FindBy(c => c.CourseId == request.CourseId);
            List<CourseUserInfo> response = new List<CourseUserInfo>();
            foreach (Courseuser courseuser in courseusers) {
                CourseUserInfo courseUserInfo = new CourseUserInfo();
                courseUserInfo.CourseuserId = courseuser.CourseId;
                courseUserInfo.IsAdmin = courseuser.IsAdmin;
                courseUserInfo.Role = courseuser.Role;
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(courseUserInfo.UserId);
                courseUserInfo.UserId = user.UserId;
                courseUserInfo.UserName = user.UserName;
                courseUserInfo.UserEmail = user.UserEmail;
                if (user.UserImage != null) {
                    courseUserInfo.UserImage = await _s3.GetObjectTemporaryUrlAsync("", user.UserImage);
                }
                else {
                    courseUserInfo.UserImage = String.Empty;
                }
            }
            return response;
        }
    }
}
