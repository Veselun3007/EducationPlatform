using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Courses.Commands.CreateCourse {
    public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Course> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Course>> Handle(CreateCourseCommand request, CancellationToken cancellationToken) {
            string courseLink;
            do {
                courseLink = Guid.NewGuid().ToString().Substring(0, 20);
            } while (await _unitOfWork.GetRepository<Course>().FirstOrDefaultAsync(c => c.CourseLink == courseLink) != null);
            Course course = new Course(request.CourseName, request.CourseDescription, courseLink);
            Course res = await _unitOfWork.GetRepository<Course>().AddAsync(course);
            //_unitOfWork.SaveChanges();
            return res;
        }
    }
}
