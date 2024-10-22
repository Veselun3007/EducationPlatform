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
            var courseusers = _unitOfWork.GetRepository<Courseuser>().FindBy(cu => cu.CourseId == request.CourseId);
            if(courseusers.Any(cu => cu.UserId == request.UserId)) return Errors.CourseuserError.Error403();
            List<CourseUserInfo> response = new List<CourseUserInfo>();
            foreach (Courseuser courseuser in courseusers) {
                CourseUserInfo courseUserInfo = new CourseUserInfo(courseuser);
                if (courseuser.User.UserImage != null) {
                    try {
                        courseUserInfo.UserImage = await _s3.GetObjectTemporaryUrlAsync(courseuser.User.UserImage);
                    }
                    catch {
                        courseUserInfo.UserImage = String.Empty;
                    }
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
