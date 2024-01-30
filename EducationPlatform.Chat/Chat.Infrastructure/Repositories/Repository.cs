using EPChat.Domain.Interfaces;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPChat.Infrastructure.Repositories
{
    internal class Repository<T>(ChatDBContext context) : 
        IRepository<T> where T : class, IEntity
    {
        private readonly ChatDBContext _context = context;
        private DbSet<T> DbSet => _context.Set<T>();

        public IQueryable<T> Get()
        {
            return DbSet;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return DbSet.Where(filter);
        }

        public T? Find(params object[] keyValues)
        {
            var entity = DbSet.Find(keyValues);

            return entity;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                DbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }

        public void Delete(params object[] keyValues)
        {
            T? entityToDelete = Find(keyValues);
            if(entityToDelete != null)
                Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            DbSet.Remove(entityToDelete);
        }

        public void DeleteRange(IEnumerable<T> entityToDelete)
        {
            DbSet.RemoveRange(entityToDelete);
        }

        public async Task<T?> GetById(int id)
        {
            return await DbSet.FirstAsync(e => e.Id == id);
        }
    }
}
