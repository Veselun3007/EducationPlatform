using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using CourseService.Infrastructure.Repositories;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser {
    public class CreateCourseuserCommandHandler : IRequestHandler<CreateCourseuserCommand, Result<Courseuser>> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseuserCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Courseuser>> Handle(CreateCourseuserCommand request, CancellationToken cancellationToken) {
            Course? course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseLink == request.CourseLink);
            User? user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId);
            if (course == null || user == null) return Errors.CourseuserError.TestError();
            Courseuser courseuser = new Courseuser() {
                Role = request.Role,
                Course = course,
                User = user,
            };
            courseuser = await _unitOfWork.GetRepository<Courseuser>().AddAsync(courseuser);
            return courseuser;
        }
    }
}
