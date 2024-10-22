using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Application.DTOs.Courses.Queries;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Application.Courses.Queries.GetCourse {
    public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, CourseInfo> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetCourseQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<CourseInfo>> Handle(GetCourseQuery request, CancellationToken cancellationToken) {
            Course course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseId == request.CourseId && c.Courseusers.Any(cu => cu.UserId == request.UserId));
            //Course course = await _unitOfWork.GetRepository<Course>().GetByIdAsync(request.CourseId);
            if (course == null) return Errors.Global.Error404();

            Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
            User admin = await _unitOfWork.GetRepository<User>().GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.Role == 0).UserId);
            if (admin == null || courseuser == null) return Errors.Global.Error404();
            CourseInfo courseInfo = new CourseInfo(course, courseuser);

            courseInfo.AdminInfo.AdminName = admin.UserName;
            if (admin.UserImage != null) {
                try {
                    courseInfo.AdminInfo.ImageLink = await _s3.GetObjectTemporaryUrlAsync(admin.UserImage);
                }
                catch {
                    courseInfo.AdminInfo.ImageLink = String.Empty;
                }
            }
            else {
                courseInfo.AdminInfo.ImageLink = String.Empty;
            }

            return courseInfo;
        }
    }
}
