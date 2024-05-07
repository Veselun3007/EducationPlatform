using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using EducationPlatform.Identity;
using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Helpers;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.Config;
using EPChat.Core.Models.ErrorModels;
using EPChat.Core.Services;
using EPChat.Domain.Entities;
using EPChat.Infrastructure;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using EPChat.Web.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Chat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            _configuration.AddSystemsManager("/education-platform/Development", new AWSOptions
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
            builder.Services.AddScoped<IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, Error>, OperationServices>();
            builder.Services.AddScoped<IQuery<MessageOutDTO, CourseUser>, QueryService>();

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

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

            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseCors("AllowAll");

            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
