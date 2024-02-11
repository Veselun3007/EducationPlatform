namespace IdentityServer.Domain.Entities
{
    public class Token
    {
        public int Id { get; set; }

        public required int UserId { get; set; }

        public required string RefreshToken { get; set; }

        public required DateTime RefreshTokenValidUntil { get; set; }

        public virtual AppUser AppUser { get; set; } = null!;
    }
}
