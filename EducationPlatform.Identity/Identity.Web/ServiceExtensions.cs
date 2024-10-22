using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Identity.Domain.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Identity.Web
{
    public static class ServiceExtensions
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
            services.AddAWSService<IAmazonCognitoIdentityProvider>();

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

        internal static void AddJwtValidation(WebApplicationBuilder builder, AwsOptions awsOptions)
        {
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
                    IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                    {
                        return GetKeys(parameters).GetAwaiter().GetResult();
                    },
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = $"https://cognito-idp.{awsOptions.Region}.amazonaws.com/{awsOptions.UserPoolId}",
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                };
            });
        }
    }
}
