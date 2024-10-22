using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using EPChat.Core.Models.Config;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EPChat.Web
{
    internal static class ServiceExtensions
    {
        internal static AWSOptions SetAWSOption()
        {
            return new AWSOptions()
            {
                Credentials = new EnvironmentVariablesAWSCredentials(),
                Region = new EnvironmentVariableAWSRegion().Region
            };
        }

        internal static IServiceCollection AddAWS(this IServiceCollection services, IConfigurationBuilder configuration)
        {
            configuration.AddSystemsManager("/to-do/Development", SetAWSOption());
            services.AddDefaultAWSOptions(SetAWSOption());
            services.AddAWSService<IAmazonS3>();

            return services;
        }

        internal static (AwsOptions awsOptions, DbOptions dbOptions) AddVariables(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection(nameof(AwsOptions)).Get<AwsOptions>() ?? new AwsOptions();
            var dbOptions = configuration.GetSection(nameof(DbOptions)).Get<DbOptions>() ?? new DbOptions();

            return (awsOptions, dbOptions);
        }

        internal static async Task<IEnumerable<SecurityKey>?> GetKeys(TokenValidationParameters parameters)
        {
            HttpClient client = new();
            var json = await client.GetStringAsync(parameters.ValidIssuer + "/.well-known/jwks.json");
            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json)?.Keys;
            return keys;
        }      
    }
}
