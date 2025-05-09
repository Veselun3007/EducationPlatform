using StudentResult.Domain.Entities;

namespace StudentResult.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IMinRepository<string, User> UserRepository { get; }

        IMinRepository<int, Assignment> AssignmentRepository { get; }

        IRepository<int, Comment> CommentRepository { get; }

        IRepository<int, CourseUser> CourseuserRepository { get; }

        IRepository<int, AttachedFile> AttachedFileRepository { get; }

        IRepository<int, StudentAssignment> StudentAssignmentRepository { get; }

        Task<int> CommitAsync();
    }
}
