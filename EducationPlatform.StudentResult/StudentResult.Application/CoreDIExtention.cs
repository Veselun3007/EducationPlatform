using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentResult.Application.Services;

namespace StudentResult.Application
{
    public static class CoreDIExtention
    {
        public static void AddCoreServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<StudentAssignmentService>();
        }
    }
}
