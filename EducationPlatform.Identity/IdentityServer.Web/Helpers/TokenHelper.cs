using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Web.Helpers
{
    internal class TokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _key;
        private readonly string _accessTokenTtlInMinutes;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = _configuration["JWT:Key"] ?? throw new("JWT:Key");
            _accessTokenTtlInMinutes = _configuration["JWT:AccessTokenTtlInMinutes"] ?? throw new("JWT:AccessTokenTtlInMinutes");
        }

        public static string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateAccessToken(string name, string email)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, name),
                new(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            int ttl = int.Parse(_accessTokenTtlInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(ttl),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
