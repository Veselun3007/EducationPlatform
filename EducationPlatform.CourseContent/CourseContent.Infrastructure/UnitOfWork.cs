using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using CourseContent.Infrastructure.Interfaces.Base;
using CourseContent.Infrastructure.Repositories.GenericRepositories;

namespace CourseContent.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;

        public UnitOfWork(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;

            AssignmentRepository = new ContentRepository<Assignment>(_dbContext);
            AssignmentfileRepository = new EntityRepository<Assignmentfile>(_dbContext);
            AssignmentlinkRepository = new MinRepository<Assignmentlink>(_dbContext);

            MaterialRepository = new ContentRepository<Material>(_dbContext);
            MaterialfileRepository = new EntityRepository<Materialfile>(_dbContext);
            MateriallinkRepository = new MinRepository<Materiallink>(_dbContext);

            TopicRepository = new Repository<Topic>(_dbContext);
        }

        public IContentRepository<Assignment> AssignmentRepository { get; private set; }
        public IContentRepository<Material> MaterialRepository { get; private set; }
        public IEntityRepository<Assignmentfile> AssignmentfileRepository { get; private set; }
        public IEntityRepository<Materialfile> MaterialfileRepository { get; private set; }
        public IMinRepository<Assignmentlink> AssignmentlinkRepository { get; private set; }
        public IMinRepository<Materiallink> MateriallinkRepository { get; private set; }
        public IRepository<Topic> TopicRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

