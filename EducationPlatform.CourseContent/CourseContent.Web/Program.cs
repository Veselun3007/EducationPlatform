using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Models.Config;
using CourseContent.Core.Models.ErrorModels;
using CourseContent.Core.Services;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Web
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

            builder.Services.AddDbContextPool<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOptions.ConnectionString);
            });

            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IBaseOperation<TopicOutDTO, Error, TopicDTO, TopicUpdateDTO>, TopicService>();
            builder.Services.AddScoped<IOperation<AssignmentOutDTO, Error, AssignmentDTO, AssignmentfileOutDTO, AssignmentUpdateDTO, Assignmentlink>, AssignmentService>();
            builder.Services.AddScoped<IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO, MaterialUpdateDTO, Materiallink>, MaterialService>();

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseCors("AllowAll");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseContent");
                });
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
