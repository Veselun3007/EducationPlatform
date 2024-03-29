using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services
{
    public class DbOperation : IBaseDbOperation<User>
    {
        private readonly EducationPlatformContext _context;
        private readonly DbSet<User> _dbSet;

        public DbOperation(EducationPlatformContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<User> AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {       
            return await _dbSet.FindAsync(id);
        }

        public async Task<User?> UpdateAsync(User entity, string id)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            await _context.SaveChangesAsync();
            return existingEntity;
        }
    }
}
