using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Application.Courseusers.Queries.GetCourseusersByCourse {
    public class GetCourseusersByCourseQueryHandler : IRequestHandler<GetCourseusersByCourseQuery, Result<IEnumerable<CourseUserInfo>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        private readonly EducationPlatformContext _db;
        public GetCourseusersByCourseQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3, EducationPlatformContext db) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
            _db = db;
        }
        public async Task<Result<IEnumerable<CourseUserInfo>>> Handle(GetCourseusersByCourseQuery request, CancellationToken cancellationToken) {
            //var courseusers = _unitOfWork.GetRepository<Courseuser>().FindBy(c => c.CourseId == request.CourseId);
            var courseusers = _db.Courseusers.Include(c => c.User).Where(c => c.CourseId == request.CourseId);
            List<CourseUserInfo> response = new List<CourseUserInfo>();
            foreach (Courseuser courseuser in courseusers) {
                CourseUserInfo courseUserInfo = new CourseUserInfo();
                courseUserInfo.CourseuserId = courseuser.CourseId;
                courseUserInfo.IsAdmin = courseuser.IsAdmin;
                courseUserInfo.Role = courseuser.Role;
                User user = _unitOfWork.GetRepository<User>().GetById(courseuser.UserId);
                courseUserInfo.UserId = user.UserId;
                courseUserInfo.UserName = user.UserName;
                courseUserInfo.UserEmail = user.UserEmail;
                if (user.UserImage != null) {
                    courseUserInfo.UserImage = await _s3.GetObjectTemporaryUrlAsync(user.UserImage);
                }
                else {
                    courseUserInfo.UserImage = String.Empty;
                }
                response.Add(courseUserInfo);
            }
            return response;
        }
    }
}
