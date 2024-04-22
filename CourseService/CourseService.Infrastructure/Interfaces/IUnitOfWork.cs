using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Interfaces {
    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext {
        TContext DbContext { get; }    
    }

    public interface IUnitOfWork : IDisposable {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    }
}
