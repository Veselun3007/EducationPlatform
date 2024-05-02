using EPChat.Domain.Interfaces;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Repositories
{
    internal class GetRepository<T> : IGetRepository<T> where T : class, IEntity
    {
        private readonly ChatDBContext _context;
        private readonly DbSet<T> _dbSet;

        internal GetRepository(ChatDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
