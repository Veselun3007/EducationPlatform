using Microsoft.EntityFrameworkCore;
using StudentResult.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace StudentResult.Infrastructure.Repositories {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
        public DbContext DbContext { get; }
        public DbSet<TEntity> DbSet { get; }

        public Repository(DbContext context) {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = DbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id) => await DbSet.FindAsync(id);

        public virtual TEntity GetById(object id) => DbSet.Find(id);

        public virtual async Task<TEntity> AddAsync(TEntity entity) => (await DbSet.AddAsync(entity)).Entity;

        public virtual TEntity Add(TEntity entity) => DbSet.Add(entity).Entity;

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

        public virtual TEntity Update(TEntity entity) => DbSet.Update(entity).Entity;

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) => DbSet.AsQueryable().Where(predicate);

        public virtual IEnumerable<TEntity> FindEnumerable(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate);

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate) => DbSet.Any(predicate);

        public virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => DbSet.FirstOrDefaultAsync(predicate);
    }
}
