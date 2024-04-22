using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.CreateCourseuser {
    public class CreateCourseuserCommandHandler : IRequestHandler<CreateCourseuserCommand, Result<Courseuser>> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseuserCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Courseuser>> Handle(CreateCourseuserCommand request, CancellationToken cancellationToken) {
            //Course? course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseLink == request.CourseLink);
            User? user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId);
            //if (course == null || user == null) return Errors.CourseuserError.TestError();
            //Courseuser courseuser = new Courseuser(course.CourseId, user.UserId, request.IsAdmin, request.Role);
            Courseuser courseuser = new Courseuser() {
                IsAdmin = request.IsAdmin,
                Role = request.Role,
                Course = request.Course,
                User = user,
            };
            Courseuser res = await _unitOfWork.GetRepository<Courseuser>().AddAsync(courseuser);
            //_unitOfWork.SaveChanges();
            return res;
        }
    }
}
