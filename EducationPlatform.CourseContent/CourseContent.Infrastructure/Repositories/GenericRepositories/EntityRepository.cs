using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class EntityRepository<T>(EducationPlatformContext dbContext) : 
        IEntityRepository<T> where T : class, IAggregateRoot
    {
        private readonly EducationPlatformContext _dbContext = dbContext;
     
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}
