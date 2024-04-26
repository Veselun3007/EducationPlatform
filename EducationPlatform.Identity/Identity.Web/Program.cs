using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Identity.Core.Helpers;
using Identity.Core.Services;
using Identity.Domain.Config;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace EducationPlatform.Identity
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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
