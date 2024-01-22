using IdentityServer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Core.Helpers
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

        public Dictionary<string, string> GenerateTokens(User user)
        {
            var _token = GenerateAccessToken(user);
            var _refreshToken = Guid.NewGuid().ToString();

            var _tokens = new Dictionary<string, string>
            {
                { "Jwt", _token },
                { "Refresh", _refreshToken }
            };

            return _tokens;
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.UserEmail)
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
