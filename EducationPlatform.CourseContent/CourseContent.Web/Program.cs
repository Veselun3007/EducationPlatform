using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Services;
using CourseContent.Infrastructure;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Interfaces;
using EducationPlatform.Identity;
using Identity.Core.Models;
using Identity.Domain.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Configuration.AddSystemsManager("/education-platform/Development", new AWSOptions
            {
                Credentials = new EnvironmentVariablesAWSCredentials(),
                Region = new EnvironmentVariableAWSRegion().Region,
            });
            builder.Services.AddAWS();

            builder.Services
                .Configure<AwsOptions>(_configuration.GetSection(nameof(AwsOptions)))
                .Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));

            var (awsOptions, dbOptions) = builder.Services.AddVariables();

            builder.Services.AddDbContextPool<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOptions.ConnectionString);
            });

            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IBaseOperation<TopicOutDTO, Error, TopicDTO>, TopicService>();
            builder.Services.AddScoped<IOperation<AssignmentOutDTO, Error, AssignmentDTO, AssignmentfileOutDTO>, AssignmentService>();
            builder.Services.AddScoped<IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO>, MaterialService>();


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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://cognito-idp.{awsOptions.Region}.amazonaws.com/{awsOptions.UserPoolId}";
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://cognito-idp.{awsOptions.Region}.amazonaws.com/{awsOptions.UserPoolId}",
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                };
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
