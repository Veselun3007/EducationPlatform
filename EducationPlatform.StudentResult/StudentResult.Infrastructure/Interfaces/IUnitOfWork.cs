using Microsoft.EntityFrameworkCore;

namespace StudentResult.Infrastructure.Interfaces {
    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext {
        TContext DbContext { get; }
    }

    public interface IUnitOfWork : IDisposable {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
