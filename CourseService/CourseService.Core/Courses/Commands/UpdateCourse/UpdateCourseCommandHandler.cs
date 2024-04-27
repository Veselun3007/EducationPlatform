using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;

namespace CourseService.Application.Courses.Commands.UpdateCourse {
    public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, CourseInfo> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public UpdateCourseCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<CourseInfo>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken) {
            Course course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseId == request.CourseId);
            if (course == null) return Errors.Global.Error404();

            User admin = await _unitOfWork.GetRepository<User>(false).GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.Role == 0).UserId);
            Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
            if (admin == null || courseuser == null) return Errors.Global.Error404();
            if (admin.UserId != request.UserId) return Errors.Global.Error403();

            course.CourseName = request.CourseName;
            course.CourseDescription = request.CourseDescription;
            course = _unitOfWork.GetRepository<Course>().Update(course);
        
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
