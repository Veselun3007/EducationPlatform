using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentResult.Application.Interfaces;
using StudentResult.Infrastructure.Context;
using StudentResult.Infrastructure.Options;
using StudentResult.Infrastructure.Repositories;
using StudentResult.Infrastructure.Services;

namespace StudentResult.Infrastructure
{
    public static class InfrastructureDIExtention
    {
        private static AWSOptions SetAWSOption()
        {
            return new AWSOptions()
            {
                Credentials = new EnvironmentVariablesAWSCredentials(),
                Region = new EnvironmentVariableAWSRegion().Region
            };
        }

        private static IServiceCollection AddAWS(this IServiceCollection services, IConfigurationBuilder configuration)
        {
            configuration.AddSystemsManager("/to-do/Development", SetAWSOption());
            services.AddDefaultAWSOptions(SetAWSOption());
            services.AddAWSService<IAmazonS3>();
            return services;
        }

        private static (AwsOptions awsOptions, DbOptions dbOptions) AddVariables(ConfigurationManager configuration)
        {
            var awsOptions = configuration.GetSection(nameof(AwsOptions)).Get<AwsOptions>() ?? new AwsOptions();
            var dbOptions = configuration.GetSection(nameof(DbOptions)).Get<DbOptions>() ?? new DbOptions();
            return (awsOptions, dbOptions);
        }

        public static AwsOptions AddInfrastructure(this IHostApplicationBuilder builder, ConfigurationManager configuration)
        {
            builder.Services.AddAWS(configuration);
            builder.Services.Configure<AwsOptions>(configuration.GetSection(nameof(AwsOptions)));
            builder.Services.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));

            var (awsOptions, dbOptions) = AddVariables(configuration);
            builder.Services.AddDbContext<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOptions.ConnectionString);
            });
            builder.Services.AddScoped<IAwsFileService, AwsFileService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            return awsOptions;
        }
    }
}
