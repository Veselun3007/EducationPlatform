namespace IdentityServer.Domain.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Salt { get; set; }

        public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
    }
}
