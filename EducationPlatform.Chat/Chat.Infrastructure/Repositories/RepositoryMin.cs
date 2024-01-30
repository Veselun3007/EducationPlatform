using EPChat.Domain.Interfaces;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Infrastructure.Repositories
{
    internal class RepositoryMin<T>(ChatDBContext context) : 
        IMinRepository<T> where T : class, IEntity
    {
        private readonly ChatDBContext _context = context;
        
        private DbSet<T> DbSet => _context.Set<T>();

        public async Task<T?> GetById(int id)
        {
            return await DbSet.FirstAsync(e => e.Id == id);
        }
    }
}
