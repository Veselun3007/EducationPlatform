using IdentityServer.Core.DTOs.Login;
using IdentityServer.Core.DTOs.Token;
using IdentityServer.Core.DTOs.User;
using IdentityServer.Core.Helpers;
using IdentityServer.Domain.Entities;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Services
{
    public class UserService(
        TokenHelper tokenHelper,
        FileHelper fileHelper)
    {
        private readonly TokenHelper _tokenHelper = tokenHelper;
        private readonly FileHelper _fileHelper = fileHelper;

        public static AppUser FromUserDtoToAppUser(UserDTO userDTO)
        {
            var salt = CryptographyHelper.GenerateSalt();
            return new AppUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                PasswordHash = CryptographyHelper.Hash(userDTO.UserPassword, salt),
                Salt = salt,
                RefreshToken = TokenHelper.GenerateRefreshToken(),
                RefreshTokenValidUntil = DateTime.UtcNow.AddDays(7)
            };
        }

        public LoginResponseDTO FromUserDtoToResponse(UserDTO userDTO, string refreshToken)
        {
            return new LoginResponseDTO
            {
                AccessToken = _tokenHelper.GenerateAccessToken(userDTO.UserName, userDTO.Email),
                RefreshToken = refreshToken
            };
        }

        public LoginResponseDTO FromLoginToResponse(AppUser user)
        {
            return new LoginResponseDTO
            {
                AccessToken = _tokenHelper.GenerateAccessToken(user.UserName!, user.Email!),
                RefreshToken = TokenHelper.GenerateRefreshToken()
            };
        }

        public GetAccessTokenResponseDTO FromRequestToResponse(AppUser user)
        {
            return new GetAccessTokenResponseDTO
            {
                AccessToken = _tokenHelper.GenerateAccessToken(user.UserName!, user.Email!)
            };
        }

        public async Task<User> ToUserEntity(UserDTO userDTO)
        {
            return new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                UserImage = userDTO.UserImage is not null ? await _fileHelper.AddFileAsync(userDTO.UserImage) : null
            };
        }

        public async Task<UserOutDTO> FromUser(User user)
        {
            return new UserOutDTO
            {
                UserName = user.UserName,
                UserImage = await _fileHelper.GetFileLink(user.UserImage)
            };
        }

    }
}
