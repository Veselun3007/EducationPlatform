using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CourseService.Infrastructure.Repositories {
    public static class UnitOfWorkServiceCollectionExtensions {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, TRepository>(this IServiceCollection services)
                 where TEntity : class
                 where TRepository : class, IRepository<TEntity> {
            services.AddScoped<IRepository<TEntity>, TRepository>();

            return services;
        }
    }
}
