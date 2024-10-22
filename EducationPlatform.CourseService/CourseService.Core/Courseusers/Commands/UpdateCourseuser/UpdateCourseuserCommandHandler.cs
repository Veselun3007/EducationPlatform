using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.UpdateCourseuser {
    public class UpdateCourseuserCommandHandler : IRequestHandler<UpdateCourseuserCommand, Result<CourseUserInfo>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public UpdateCourseuserCommandHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<CourseUserInfo>> Handle(UpdateCourseuserCommand request, CancellationToken cancellationToken) {
            //Courseuser admin = await _unitOfWork.GetRepository<Courseuser>().GetByIdAsync(request.AdminId);
            Courseuser courseuser = await _unitOfWork.GetRepository<Courseuser>().FirstOrDefaultAsync(cu => cu.CourseuserId == request.CourseuserId);
            if (courseuser == null) return Errors.Global.Error404();
            Courseuser admin = await _unitOfWork.GetRepository<Courseuser>().FirstOrDefaultAsync(cu => 
                (cu.UserId == request.UserId) && 
                (cu.Role == 0) && 
                (cu.CourseId == courseuser.CourseId) && 
                (cu.UserId != courseuser.UserId));

            if (admin == null) return Errors.Global.Error403();

            courseuser.Role = request.Role;
            courseuser = _unitOfWork.GetRepository<Courseuser>().Update(courseuser);

            CourseUserInfo courseUserInfo = new CourseUserInfo(courseuser); //?
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

            return courseUserInfo;
        }
    }
}
