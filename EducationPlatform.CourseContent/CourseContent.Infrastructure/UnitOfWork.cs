using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using CourseContent.Infrastructure.Repositories;

namespace CourseContent.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;

        public UnitOfWork(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;

            AssignmentRepository = new AssignmentRepository(_dbContext);
            AssignmentfileRepository = new AssignmentfileRepository(_dbContext);
            AssignmentlinkRepository = new AssignmentlinkRepository(_dbContext);

            MaterialRepository = new MaterialRepository(_dbContext);
            MaterialfileRepository = new MaterialfileRepository(_dbContext);
            MateriallinkRepository = new MateriallinkRepository(_dbContext);

            TopicRepository = new TopicRepository(_dbContext);
        }

        public IContentRepository<Assignment, int> AssignmentRepository { get; private set; }
        public IContentRepository<Material, int> MaterialRepository { get; private set; }
        public IEntityRepository<Assignmentfile, int> AssignmentfileRepository { get; private set; }
        public IEntityRepository<Materialfile, int> MaterialfileRepository { get; private set; }
        public IMinRepository<Assignmentlink, int> AssignmentlinkRepository { get; private set; }
        public IMinRepository<Materiallink, int> MateriallinkRepository { get; private set; }
        public IRepository<Topic, int> TopicRepository { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

