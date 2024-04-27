using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;

namespace CourseService.Application.Courses.Queries.GetAllCourses {
    public class GetAllCoursesQueryHandler : IQueryHandler<GetAllCoursesQuery, IEnumerable<CourseInfo>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<IEnumerable<CourseInfo>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
            var courses = _unitOfWork.GetRepository<Course>().FindBy(c => c.Courseusers.Any(u => u.UserId == request.UserId)).ToList();

            List<CourseInfo> response = new List<CourseInfo>();
      
            foreach (var course in courses) {
                Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
                var admin_courseuser = course.Courseusers.FirstOrDefault(cu => cu.Role == 0);
                if (admin_courseuser == null) return Errors.Global.Error404();
                User admin = await _unitOfWork.GetRepository<User>(false).GetByIdAsync(admin_courseuser.UserId);
                //User admin = await _unitOfWork.GetRepository<User>(false).FirstOrDefaultAsync(u => u.UserId == course.Courseusers.FirstOrDefault(cu => cu.Role == 0).UserId);
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

                response.Add(courseInfo);
            }
            return response;
        }
    }
}
