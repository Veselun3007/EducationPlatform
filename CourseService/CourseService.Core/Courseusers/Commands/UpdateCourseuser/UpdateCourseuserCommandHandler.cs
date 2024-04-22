using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.UpdateCourseuser {
    public class UpdateCourseuserCommandHandler : IRequestHandler<UpdateCourseuserCommand, Result<Courseuser>> {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseuserCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Courseuser>> Handle(UpdateCourseuserCommand request, CancellationToken cancellationToken) {
            Courseuser courseuser = await _unitOfWork.GetRepository<Courseuser>().GetByIdAsync(request.CourseuserId);
            courseuser.IsAdmin = request.IsAdmin;
            courseuser.Role = request.Role;
            Courseuser res;
            try {
                res = _unitOfWork.GetRepository<Courseuser>().Update(courseuser);
                _unitOfWork.SaveChanges();
            }
            catch (Exception e) {
                return Errors.CourseuserError.TestError();
            }
            return res;
        }
    }
}
