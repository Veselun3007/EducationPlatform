using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateStudent {
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<Courseuser>> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Courseuser>> Handle(CreateStudentCommand request, CancellationToken cancellationToken) {
            Course? course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseLink == request.CourseLink);
            User? user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId);
            if (course == null || user == null) return Errors.CourseuserError.TestError();
            if (_unitOfWork.GetRepository<Courseuser>().Any(cu => cu.UserId == user.UserId && cu.CourseId == course.CourseId)) return Errors.CourseuserError.TestError();
            Courseuser courseuser = new Courseuser() {
                IsAdmin = request.IsAdmin,
                Role = request.Role,
                Course = course,
                User = user,
            };
            courseuser = await _unitOfWork.GetRepository<Courseuser>().AddAsync(courseuser);
            return courseuser;
        }
    }
}
