using Identity.Core.Helpers;
using Identity.Core.Services;
using Identity.Domain.Config;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Services.AddAWS(_configuration);
            builder.Services.Configure<AwsOptions>(_configuration.GetSection(nameof(AwsOptions)))
                            .Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));

            var (awsOptions, dbOptions) = ServiceExtensions.AddVariables(_configuration);

            builder.Services.AddDbContext<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOptions.ConnectionString);
            });

            builder.Services.AddScoped<IBaseDbOperation<User>, DbOperation>();
            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IdentityService>();

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
