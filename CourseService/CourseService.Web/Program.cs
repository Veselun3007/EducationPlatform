using Amazon.Runtime;
using CourseService.Application;
using CourseService.Application.Behaviors;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Repositories;
using CourseService.Web.Middlewares;
using FileAWS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using CourseService.Infrastructure.AWS;

//using Amazon.Extensions.NETCore.Setup;
//using Amazon.S3;

namespace CourseService.Web {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            //builder.Configuration.AddSystemsManager("/education-platform/Development", new AWSOptions {
            //    Credentials = new EnvironmentVariablesAWSCredentials(),
            //    Region = new EnvironmentVariableAWSRegion().Region,
            //});
            //builder.Services.AddAWS();
            //builder.Services
            //.Configure<AwsOptions>(_configuration.GetSection(nameof(AwsOptions)))
            //    .Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));
            //var (awsOptions, dbOptions) = builder.Services.AddVariables();

            //builder.Services.AddMediatR(cfg => {
            //    cfg.RegisterServicesFromAssembly(CourseService.Application.AssemblyReference.Assembly);
            //    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //    cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            //});
            //builder.Services.AddValidatorsFromAssembly(CourseService.Application.AssemblyReference.Assembly);

            builder.Services.AddApplication();

            builder.Services.AddS3();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ExceptionHandlingMiddleware>();

            string ep_connection = _configuration.GetConnectionString("EducationPlatformConnection") ?? "defaultConnectionString";
            builder.Services
                .AddDbContext<EducationPlatformContext>(opt => opt.UseNpgsql(ep_connection))
                .AddUnitOfWork<EducationPlatformContext>();

            builder.Services.AddScoped<AmazonS3>();

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.Authority = $"https://cognito-idp.us-east-1.amazonaws.com/us-east-1_PlemC1CS5";
                options.TokenValidationParameters = new() {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://cognito-idp.us-east-1.amazonaws.com/us-east-1_PlemC1CS5",
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                };
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
    //public class AwsOptions {
    //    public string Region { get; set; } = String.Empty;
    //    public string ClientId { get; set; } = String.Empty;
    //    public string UserPoolId { get; set; } = String.Empty;
    //    public string BucketName { get; set; } = String.Empty;
    //}
    //public class DbOptions {
    //    public string ConnectionString { get; set; } = String.Empty;
    //}
    //public static class ServiceExtensions {
    //    public static IServiceCollection AddAWS(this IServiceCollection services) {
    //        var awsOptions = new AWSOptions() {
    //            Credentials = new EnvironmentVariablesAWSCredentials(),
    //            Region = new EnvironmentVariableAWSRegion().Region
    //        };
    //        services.AddDefaultAWSOptions(awsOptions);
    //        services.AddAWSService<AmazonS3Client>();

    //        return services;
    //    }

    //    public static (AwsOptions, DbOptions) AddVariables(this IServiceCollection services) {
    //        var serviceProvider = services.BuildServiceProvider();

    //        var awsOptions = serviceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;
    //        var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;

    //        return (awsOptions, dbOptions);
    //    }
    //}
}
