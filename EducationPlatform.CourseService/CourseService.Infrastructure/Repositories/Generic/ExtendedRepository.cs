using CourseService.Application.Interfaces;
using CourseService.Domain.Base;
using CourseService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories.Generic
{
    internal class ExtendedRepository<TKey, TEntity> : Repository<TKey, TEntity>,
        IExtendedRepository<TKey, TEntity> where TEntity : AggregateRoot<TKey>
    {
        public ExtendedRepository(EducationPlatformContext dbContext) : base(dbContext) { }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
    }
}
