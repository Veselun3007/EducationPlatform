using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IContentRepository<Assignment> AssignmentRepository { get; }
        IContentRepository<Material> MaterialRepository { get; }
        IEntityRepository<Assignmentfile> AssignmentfileRepository { get; }
        IEntityRepository<Materialfile> MaterialfileRepository { get; }
        IRepository<Topic> TopicRepository { get; }

        Task<int> CompleteAsync();
    }
}
