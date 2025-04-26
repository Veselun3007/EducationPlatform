using CourseService.Application.Mappings;
using CourseService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CourseService.Application
{
    public static class CoreDIExtention
    {
        public static void AddCoreServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<SpecificMapper>();
            builder.Services.AddScoped<CoursesService>();
            builder.Services.AddScoped<CourseuserService>();
        }
    }
}
