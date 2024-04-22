using CourseService.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace CourseService.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
            return services;
        }
    }
}
