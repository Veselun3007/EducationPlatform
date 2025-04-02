using CourseService.Application;
using CourseService.Application.Services;
using CourseService.Domain.Config;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using CourseService.Infrastructure.Repositories;
using CourseService.Web.Middlewares;
using Microsoft.EntityFrameworkCore;


namespace CourseService.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Services.AddAWS(_configuration);
            builder.Services
                .Configure<AwsOptions>(_configuration.GetSection(nameof(AwsOptions)))
                .Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));

            var (awsOptions, dbOptions) = ServiceExtensions.AddVariables(_configuration);

            builder.Services.AddApplication();

            //builder.Services.AddS3();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<GlobalExceptionHandler>();

            builder.Services.AddDbContext<EducationPlatformContext>(opt => opt.UseNpgsql(dbOptions.ConnectionString));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CoursesService>();
            builder.Services.AddScoped<CourseuserService>();
            //builder.Services.AddScoped<AmazonS3>(); //delete

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));



            var app = builder.Build();
            app.UseCors("AllowAll");
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.MapControllers();
            app.Run();
        }
    }
}
