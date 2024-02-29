using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Identity.Domain.Config;
using Microsoft.Extensions.Options;

namespace EducationPlatform.Identity
{
    public static class ServiceExtensions
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

        public static (AwsOptions, DbOptions) AddVariables(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var awsOptions = serviceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;

            return (awsOptions, dbOptions);
        }
    }
}
