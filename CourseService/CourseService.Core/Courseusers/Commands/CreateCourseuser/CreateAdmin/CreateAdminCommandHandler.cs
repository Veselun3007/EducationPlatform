using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateTeacher {
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result<NewCourseInfo>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public CreateAdminCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<NewCourseInfo>> Handle(CreateAdminCommand request, CancellationToken cancellationToken) {
            User? user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId);
            if (request.NewCourseInfo.Course == null || user == null) return Errors.CourseuserError.Error404();
            
            Courseuser courseuser = new Courseuser() {
                Role = 0,
                Course = request.NewCourseInfo.Course,
                User = user,
            };
            courseuser = await _unitOfWork.GetRepository<Courseuser>().AddAsync(courseuser);
            request.NewCourseInfo.UserInfo = courseuser;

            if (user.UserImage != null) {
                try {
                    request.NewCourseInfo.AdminInfo.ImageLink = await _s3.GetObjectTemporaryUrlAsync(user.UserImage);
                }
                catch {
                    request.NewCourseInfo.AdminInfo.ImageLink = String.Empty;
                }
            }
            else {
                request.NewCourseInfo.AdminInfo.ImageLink = String.Empty;
            }
            request.NewCourseInfo.AdminInfo.AdminName = user.UserName;

            return request.NewCourseInfo;
        }
    }
}
