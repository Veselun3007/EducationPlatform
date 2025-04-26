using CourseService.Application.Interfaces.Base;
using CourseService.Domain.Entities;

namespace CourseService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IMinRepository<string, User> UserRepository { get; }

        IExtendedRepository<int, Courseuser> CourseuserRepository { get; }

        IRepository<int, Course> CourseRepository { get; }

        Task<int> CommitAsync();
    }
}
