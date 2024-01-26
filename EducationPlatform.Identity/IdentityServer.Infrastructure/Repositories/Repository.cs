using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly EducationPlatformContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(EducationPlatformContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }

        public virtual void DeleteAsync(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State is EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual async Task<T?> UpdateAsync(T entity, int id)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }
    }
}
