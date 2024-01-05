using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories.GenericRepositories;
using CourseContent.Infrastructure.Repositories.Interfaces;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;

        public UnitOfWork(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;

            AssignmentRepository = new ContentRepository<Assignment>(_dbContext);
            AssignmentfileRepository = new Repository<Assignmentfile>(_dbContext);

            MaterialRepository = new ContentRepository<Material>(_dbContext);
            MaterialfileRepository = new Repository<Materialfile>(_dbContext);
        }

        public IContentRepository<Assignment> AssignmentRepository { get; private set; }
        public IContentRepository<Material> MaterialRepository { get; private set; }
        public IRepository<Assignmentfile> AssignmentfileRepository { get; private set; }
        public IRepository<Materialfile> MaterialfileRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }
}

