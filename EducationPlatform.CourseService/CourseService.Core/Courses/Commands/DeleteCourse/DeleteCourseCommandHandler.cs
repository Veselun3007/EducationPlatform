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
            Course course = await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseId == request.CourseId);
            if (course == null) return Errors.Global.Error404();
            Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
            if (courseuser == null) return Errors.Global.Error404();
            if (courseuser.Role != 0) return Errors.Global.Error403();
            _unitOfWork.GetRepository<Course>().Delete(course);
            return Result.Ok();
        }
    }
}
