using CourseService.Application.Abstractions;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using MediatR;

namespace CourseService.Application.Courseusers.Commands.DeleteCourseuser
{
    public class DeleteCourseuserCommandHandler : IRequestHandler<DeleteCourseuserCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCourseuserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCourseuserCommand request, CancellationToken cancellationToken)
        {
            Courseuser userToDelete = await _unitOfWork.GetRepository<Courseuser>().GetByIdAsync(request.CourseuserId);
            if (userToDelete == null) return Errors.CourseuserError.Error404();
            Courseuser issuer = await _unitOfWork.GetRepository<Courseuser>().FirstOrDefaultAsync(cu =>
                (cu.CourseId == userToDelete.CourseId) &&
                (cu.UserId == request.UserId));

            if (issuer == null) return Errors.CourseuserError.Error404();

            if ((issuer.CourseuserId == userToDelete.CourseuserId && userToDelete.Role != 0) || (issuer.Role == 0 && userToDelete.Role != 0))
            {
                _unitOfWork.GetRepository<Courseuser>().Delete(userToDelete);
                return Result.Ok();
            }
            else
            {
                return Errors.CourseuserError.Error403();
            }
            //if (courseuser.Role == 0) return Errors.CourseuserError.Error403();


        }
    }
}
