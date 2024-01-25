using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Interfaces;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class Repository<T>(EducationPlatformContext context) 
        : IRepository<T> where T : class
    {
        private readonly EducationPlatformContext _context = context;

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<T?> UpdateAsync(T entity, int id)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }
    }
}
