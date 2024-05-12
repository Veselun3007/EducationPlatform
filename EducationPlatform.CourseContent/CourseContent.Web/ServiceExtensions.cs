using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using CourseContent.Core.Models.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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

            return services;
        }

        public static (AwsOptions, DbOptions) AddVariables(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var awsOptions = serviceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;

            return (awsOptions, dbOptions);
        }

        public static async Task<IEnumerable<SecurityKey>?> GetKeys(TokenValidationParameters parameters)
        {
            HttpClient client = new();
            var json = await client.GetStringAsync(parameters.ValidIssuer + "/.well-known/jwks.json");
            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json)?.Keys;
            return keys;
        }
    }
}
