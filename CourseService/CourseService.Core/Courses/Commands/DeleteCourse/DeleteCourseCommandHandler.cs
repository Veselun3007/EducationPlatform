using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Courses.Commands.DeleteCourse {
    public class DeleteCourseCommandHandler : ICommandHandler<DeleteCourseCommand> {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken) {
            Course course = await _unitOfWork.GetRepository<Course>().GetByIdAsync(request.CourseId);
            if (course == null) return Errors.CourseError.TestError();
            _unitOfWork.GetRepository<Course>().Delete(course);
            _unitOfWork.SaveChanges();
            return Result.Ok();
        }
    }
}
