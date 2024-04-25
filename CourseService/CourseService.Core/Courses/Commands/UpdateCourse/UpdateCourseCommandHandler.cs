using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Courses.Commands.UpdateCourse {
    public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, Course> {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Course>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken) {
            Course course = await _unitOfWork.GetRepository<Course>().GetByIdAsync(request.CourseId);
            if(course == null) return Errors.CourseError.TestError();
            course.CourseName = request.CourseName;
            course.CourseDescription = request.CourseDescription;
            course = _unitOfWork.GetRepository<Course>().Update(course);
            return course;
        }
    }
}
