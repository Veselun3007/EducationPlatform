using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
