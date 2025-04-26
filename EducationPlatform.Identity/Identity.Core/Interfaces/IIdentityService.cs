using Identity.Core.DTO.Responses;

namespace Identity.Core.Interfaces
{
    public interface IIdentityService
    {
        Task ComfirmUserAsync(string email, string code);
        Task DeleteAsync(string id);
        Task<string> RefreshTokensAsync(string refreshToken, string email);
        Task ResetPassword(string email, string code, string password);
        Task SendPasswordResetEmail(string email);
        Task<TokenResponse> SignInAsync(string email, string password);
        Task SignOutAsync(string accessToken);
        Task<string> SignUpAsync(string email, string password);
    }
}