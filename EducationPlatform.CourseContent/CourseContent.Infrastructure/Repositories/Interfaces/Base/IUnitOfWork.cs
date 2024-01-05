using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IContentRepository<Assignment> AssignmentRepository { get; }
        IContentRepository<Material> MaterialRepository { get; }
        IRepository<Assignmentfile> AssignmentfileRepository { get; }
        IRepository<Materialfile> MaterialfileRepository { get; }

        Task<int> CompleteAsync();
    }
}
