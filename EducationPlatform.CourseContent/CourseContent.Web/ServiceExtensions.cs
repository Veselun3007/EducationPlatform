using CourseContent.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CourseContent.Web
{
    internal static class ServiceExtensions
    {
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
