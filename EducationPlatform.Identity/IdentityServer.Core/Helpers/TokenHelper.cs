using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Core.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _signingCredentials;
        private readonly DateTime _expires;
        private readonly string _jwtKey;
        private readonly string _accessTokenTtlInMinutes;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _ttl;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtKey = _configuration["JWT:Key"]!;
            _accessTokenTtlInMinutes = _configuration["JWT:AccessTokenTtlInMinutes"]!;
            _issuer = _configuration["JWT:Issuer"]!;
            _audience = _configuration["JWT:Audience"]!;
            _ttl = int.Parse(_accessTokenTtlInMinutes);
            _expires = DateTime.UtcNow.AddMinutes(_ttl);
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            _signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
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
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: _expires,
                claims: claims,
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
