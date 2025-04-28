using Chat.Core.Interfaces.Infrastructure;
using Chat.Domain.Entities.Base;
using Chat.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Repositories
{
    public abstract class RepositoryMin<T, TKey> : IMinRepository<T, TKey> where T : BaseEntity<TKey>
    {
        protected readonly EducationPlatformContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryMin(EducationPlatformContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
