using EPChat.Domain.Interfaces;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class, IEntity
    {

        private readonly ChatDBContext _context;
        private readonly DbSet<T> _dbSet;

        internal Repository(ChatDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T?> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task RemoveRangeAsync(List<int> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstAsync(e => e.Id == id);
        }
    }
}
