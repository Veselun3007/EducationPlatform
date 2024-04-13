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
        IEntityRepository<Assignmentlink> AssignmentlinkRepository { get; }
        IEntityRepository<Materiallink> MateriallinkRepository { get; }
        IRepository<Topic> TopicRepository { get; }
        Task<int> CompleteAsync();
    }
}
