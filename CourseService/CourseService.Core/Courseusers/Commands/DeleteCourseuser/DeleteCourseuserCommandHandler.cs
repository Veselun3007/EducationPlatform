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
            try {
                _unitOfWork.GetRepository<Courseuser>().Delete(courseuser);
                _unitOfWork.SaveChanges();
            }
            catch (Exception e) {
                return Errors.CourseuserError.TestError();
            }
            return null;
        }
    }
}
