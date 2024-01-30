using CourseContent.Domain.Interfaces;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Repositories.GenericRepositories
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IAggregateRoot
    {
        private readonly EducationPlatformContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(EducationPlatformContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity =  await _dbSet.FindAsync(id);
            return entity!;
        }
    }
}
