using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;

namespace CourseContent.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IContentRepository<Assignment, int> AssignmentRepository { get; }

        IContentRepository<Material, int> MaterialRepository { get; }

        IEntityRepository<Assignmentfile, int> AssignmentfileRepository { get; }

        IEntityRepository<Materialfile, int> MaterialfileRepository { get; }

        IMinRepository<Assignmentlink, int> AssignmentlinkRepository { get; }

        IMinRepository<Materiallink, int> MateriallinkRepository { get; }

        IContentRepository<Topic, int> TopicRepository { get; }

        Task<int> CommitAsync();
    }
}
