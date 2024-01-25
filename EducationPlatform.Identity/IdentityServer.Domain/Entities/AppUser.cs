using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string Salt { get; set; }

        public required string RefreshToken { get; set; }

        public required DateTime RefreshTokenValidUntil { get; set; }
    }
}
