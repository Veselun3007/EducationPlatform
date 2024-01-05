using CourseContent.Core.Interfaces;
using CourseContent.Core.Services;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories;
using CourseContent.Infrastructure.Repositories.GenericRepositories;
using CourseContent.Infrastructure.Repositories.Interfaces;
using CourseContent.Infrastructure.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CourseContent.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            var _configuration = builder.Configuration;
            string epConnection = _configuration.GetConnectionString("EducationPlatformConnection") ?? "defaultConnectionString";

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            builder.Services.AddDbContextPool<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(epConnection);
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IOperation<Material>, MaterialService>();
            builder.Services.AddScoped<IOperation<Assignment>, AssignmentService>();     

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
