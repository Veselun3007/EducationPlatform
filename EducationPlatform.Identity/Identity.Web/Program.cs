using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Identity.Core.Helpers;
using Identity.Core.Services;
using Identity.Domain.Config;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EducationPlatform.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAWS(this IServiceCollection services)
        {
            var awsOptions = new AWSOptions()
            {
                Credentials = new EnvironmentVariablesAWSCredentials(),
                Region = new EnvironmentVariableAWSRegion().Region
            };
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<AmazonS3Client>();
            services.AddAWSService<IAmazonCognitoIdentityProvider>();
          
            return services;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            
            builder.Configuration.AddSystemsManager("/education-platform/Development", new AWSOptions
            {
                Credentials = new EnvironmentVariablesAWSCredentials(),
                Region = new EnvironmentVariableAWSRegion().Region,
            });
            builder.Services.AddAWS();

            builder.Services.AddOptions<AwsOptions>()
                .Bind(_configuration.GetSection(nameof(AwsOptions)));

            builder.Services.AddOptions<DbOptions>()
                 .Bind(_configuration.GetSection(nameof(DbOptions)));

            var serviceProvider = builder.Services.BuildServiceProvider();
            var awsOptions = serviceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;
            var dbOption = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;

            builder.Services.AddDbContext<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOption.ConnectionString);
            });

            builder.Services.AddScoped<IBaseDbOperation<User>, DbOperation>();
            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<UserOperation>();
            builder.Services.AddScoped<IdentityOperation>();

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

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
                    ValidateAudience = false
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
