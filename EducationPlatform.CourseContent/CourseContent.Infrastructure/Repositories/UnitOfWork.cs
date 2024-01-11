using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Helpers;
using CourseContent.Infrastructure.Repositories.GenericRepositories;
using CourseContent.Infrastructure.Repositories.Interfaces;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;
        private readonly FilesHelper _filesHelper;

        public UnitOfWork(EducationPlatformContext dbContext, FilesHelper filesHelper)
        {
            _dbContext = dbContext;
            _filesHelper = filesHelper;

            AssignmentRepository = new ContentRepository<Assignment>(_dbContext, _filesHelper);
            AssignmentfileRepository = new EntityRepository<Assignmentfile>(_dbContext);

            MaterialRepository = new ContentRepository<Material>(_dbContext, _filesHelper);
            MaterialfileRepository = new EntityRepository<Materialfile>(_dbContext);
        }

        public IContentRepository<Assignment> AssignmentRepository { get; private set; }
        public IContentRepository<Material> MaterialRepository { get; private set; }
        public IEntityRepository<Assignmentfile> AssignmentfileRepository { get; private set; }
        public IEntityRepository<Materialfile> MaterialfileRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

