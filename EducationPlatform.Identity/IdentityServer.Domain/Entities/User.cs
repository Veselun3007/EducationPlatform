namespace IdentityServer.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public required string UserName { get; set; }

        public required string UserEmail { get; set; }

        public string? UserImage { get; set; }
    }
}
