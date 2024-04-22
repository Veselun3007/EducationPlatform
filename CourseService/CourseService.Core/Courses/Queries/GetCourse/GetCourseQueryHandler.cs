using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Application.DTOs.Courses.Queries;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;

namespace CourseService.Application.Courses.Queries.GetCourse {
    public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, CourseInfo> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetCourseQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<CourseInfo>> Handle(GetCourseQuery request, CancellationToken cancellationToken) {
            Course course = await _unitOfWork.GetRepository<Course>().GetByIdAsync(request.CourseId);

            CourseInfo courseInfo = new CourseInfo();
            courseInfo.Course = course;

            User admin = await _unitOfWork.GetRepository<User>().GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.IsAdmin == true).UserId);
            courseInfo.AdminInfo.AdminName = admin.UserName;
            if (admin.UserImage != null) {
                courseInfo.AdminInfo.ImageLink = await _s3.GetObjectTemporaryUrlAsync("", admin.UserImage);
            }
            else {
                courseInfo.AdminInfo.ImageLink = String.Empty;
            }

            Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
            courseInfo.UserInfo.IsAdmin = courseuser.IsAdmin;
            courseInfo.UserInfo.Role = courseuser.Role;

            return courseInfo;
        }
    }
}
