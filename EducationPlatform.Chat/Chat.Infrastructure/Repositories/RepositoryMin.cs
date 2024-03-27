using EPChat.Domain.Interfaces;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Infrastructure.Repositories
{
    internal class RepositoryMin<T> : IMinRepository<T> where T : class, IEntity
    {
        private readonly ChatDBContext _context;
        private readonly DbSet<T> _dbSet;

        internal RepositoryMin(ChatDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstAsync(e => e.Id == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
    }
}
