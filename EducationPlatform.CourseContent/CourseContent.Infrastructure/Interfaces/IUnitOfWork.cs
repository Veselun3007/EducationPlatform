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

        IMinRepository<Assignmentlink> AssignmentlinkRepository { get; }
        
        IMinRepository<Materiallink> MateriallinkRepository { get; }
        
        IRepository<Topic> TopicRepository { get; }

        Task<int> CommitAsync();
    }
}
