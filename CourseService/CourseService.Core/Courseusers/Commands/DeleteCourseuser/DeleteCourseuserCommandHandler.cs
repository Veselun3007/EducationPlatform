using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.DeleteCourseuser {
    public class DeleteCourseuserCommandHandler : IRequestHandler<DeleteCourseuserCommand, Result> {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCourseuserCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCourseuserCommand request, CancellationToken cancellationToken) {       
            Courseuser courseuser = await _unitOfWork.GetRepository<Courseuser>().GetByIdAsync(request.CourseuserId);
            if (courseuser == null) return Errors.CourseuserError.Error404();
            Courseuser admin = await _unitOfWork.GetRepository<Courseuser>().FirstOrDefaultAsync(cu => 
                (cu.CourseId == courseuser.CourseId) && 
                (cu.UserId == request.UserId) && 
                (cu.Role == 0) && 
                (cu.UserId != courseuser.UserId));
            
            if (admin == null) return Errors.CourseuserError.Error403();
            //if (courseuser.Role == 0) return Errors.CourseuserError.Error403();

            _unitOfWork.GetRepository<Courseuser>().Delete(courseuser);
            return Result.Ok();
        }
    }
}
