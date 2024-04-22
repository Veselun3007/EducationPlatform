using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseService.Infrastructure.Repositories {
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext {
        public TContext DbContext { get; }
        private Dictionary<Type, object>? _repositories;
        public UnitOfWork(TContext context) {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class {
            if (hasCustomRepository) {
                var customRepo = DbContext.GetService<IRepository<TEntity>>();
                if (customRepo != null) {
                    return customRepo;
                }
            }

            _repositories ??= new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) {
                _repositories[type] = new Repository<TEntity>(DbContext);
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges() {
            try {
                return DbContext.SaveChanges();
            }
            catch (Exception) {
                return 0;
            }
        }
        public async Task<int> SaveChangesAsync() {
            try {
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception) {
                return 0;
            }
        }

        public void Dispose() {
            DbContext.Dispose();
        }
    }
}
