using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces.Base;

namespace CourseService.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IMinRepository<string, User> UserRepository { get; }

        IExtendedRepository<int, Courseuser> CourseuserRepository { get; }

        IRepository<int, Course> CourseRepository { get; }

        Task<int> CommitAsync();
    }
}
