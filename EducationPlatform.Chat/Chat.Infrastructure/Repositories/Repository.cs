using Chat.Core.Interfaces.Infrastructure;
using Chat.Domain.Entities.Base;
using Chat.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Repositories
{
    public class Repository<T, TKey> : RepositoryMin<T, TKey>, IRepository<T, TKey> where T : BaseEntity<TKey>
    {
        internal Repository(EducationPlatformContext context) : base(context) { }

        public async Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AsNoTracking().Where(filter).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T?> UpdateAsync(TKey id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if(existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<T>?> GetEntitiesAsync(Expression<Func<T, bool>>? filter = null,             
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? take = null, 
            params Expression<Func<T, object>>[]? includes) 
        {
            var query = _dbSet.AsQueryable().AsNoTracking();

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }

            if(take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }
    }
}
