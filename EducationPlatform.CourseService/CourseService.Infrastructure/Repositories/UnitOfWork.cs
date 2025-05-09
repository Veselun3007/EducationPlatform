using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Repositories.SpecificRepositories;

namespace CourseService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EducationPlatformContext _dbContext;
        public UnitOfWork(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;

            UserRepository = new UserRepository(_dbContext);
            CourseuserRepository = new CourseuserRepository(_dbContext);
            CourseRepository = new CourseRepository(_dbContext);

        }

        public IMinRepository<string, User> UserRepository { get; private set; }
        public IExtendedRepository<int, Courseuser> CourseuserRepository { get; private set; }
        public IRepository<int, Course> CourseRepository { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
