namespace IdentityServer.Core.DTOs.Login
{
    public class LoginResponseDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
