using StudentResult.Application.Interfaces;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Context;

namespace StudentResult.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;

        public UnitOfWork(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(_dbContext);
            CommentRepository = new CommentRepository(_dbContext);
            CourseuserRepository = new CourseuserRepository(_dbContext);
            AssignmentRepository = new AssignmentRepository(_dbContext);
            AttachedFileRepository = new AttachedFileRepository(_dbContext);
            StudentAssignmentRepository = new StudentAssignmentRepository(_dbContext);
        }

        public IRepository<int, StudentAssignment> StudentAssignmentRepository { get; private set; }

        public IRepository<int, AttachedFile> AttachedFileRepository { get; private set; }

        public IRepository<int, CourseUser> CourseuserRepository { get; private set; }

        public IRepository<int, Comment> CommentRepository { get; private set; }

        public IMinRepository<string, User> UserRepository { get; private set; }

        public IMinRepository<int, Assignment> AssignmentRepository { get; private set; }  

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
