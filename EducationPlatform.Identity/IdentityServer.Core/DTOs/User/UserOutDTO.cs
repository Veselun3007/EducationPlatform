namespace IdentityServer.Core.DTOs.User
{
    public class UserOutDTO
    {
        public required string UserName { get; set; }

        public string? UserImage { get; set; }
    }
}
