using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateTeacher {
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result<Courseuser>> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateAdminCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Courseuser>> Handle(CreateAdminCommand request, CancellationToken cancellationToken) {
            User? user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId);
            if (request.Course == null || user == null) return Errors.CourseuserError.TestError();
            Courseuser courseuser = new Courseuser() {
                IsAdmin = request.IsAdmin,
                Role = request.Role,
                Course = request.Course,
                User = user,
            };
            courseuser = await _unitOfWork.GetRepository<Courseuser>().AddAsync(courseuser);
            return courseuser;
        }
    }
}
