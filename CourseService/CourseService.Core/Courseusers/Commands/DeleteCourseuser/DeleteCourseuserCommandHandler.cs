using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.DeleteCourseuser {
    public class DeleteCourseuserCommandHandler : IRequestHandler<DeleteCourseuserCommand, Result<Object>> {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCourseuserCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Object>> Handle(DeleteCourseuserCommand request, CancellationToken cancellationToken) {
            Courseuser courseuser = await _unitOfWork.GetRepository<Courseuser>().GetByIdAsync(request.CourseuserId);
            if (courseuser == null) return Errors.CourseuserError.TestError();
            if (courseuser.IsAdmin == true) return Errors.CourseuserError.TestError();
            _unitOfWork.GetRepository<Courseuser>().Delete(courseuser);
            return true;
        }
    }
}
