namespace IdentityServer.Domain.Entities
{
    public class LoginModel
    {
        public required string Email { get; set; }
        public required string UserPassword { get; set; }
    }
}
