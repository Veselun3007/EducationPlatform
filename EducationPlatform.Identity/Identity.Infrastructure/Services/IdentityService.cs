using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Identity.Core.DTO.Responses;
using Identity.Core.Interfaces;
using Identity.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoService;
        private readonly AwsOptions _options;
        public IdentityService(IAmazonCognitoIdentityProvider cognitoService, IOptions<AwsOptions> option)
        {
            _options = option.Value;
            _cognitoService = cognitoService;
        }

        public async Task<string> SignUpAsync(string email, string password)
        {
            var signUpRequest = new SignUpRequest()
            {
                ClientId = _options.ClientId,
                SecretHash = CreatSecretHash(email),
                Username = email,
                Password = password
            };

            var signUpResponse = await _cognitoService.SignUpAsync(signUpRequest);
            return signUpResponse.UserSub;
        }

        public async Task<TokenResponse> SignInAsync(string email, string password)
        {
            Dictionary<string, string> authParams = new()
            {
                {"USERNAME", email},
                {"PASSWORD", password},
                {"SECRET_HASH", CreatSecretHash(email)}
            };

            InitiateAuthRequest request = new()
            {
                ClientId = _options.ClientId,
                AuthParameters = authParams,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            };

            var response = await _cognitoService.InitiateAuthAsync(request);
            return CreateResponse(response.AuthenticationResult.AccessToken, response.AuthenticationResult.RefreshToken);
        }

        public async Task ComfirmUserAsync(string email, string code)
        {
            var comfirmationRequest = new ConfirmSignUpRequest()
            {
                Username = email,
                ConfirmationCode = code,
                SecretHash = CreatSecretHash(email),
                ClientId = _options.ClientId,
            };

            await _cognitoService.ConfirmSignUpAsync(comfirmationRequest);
        }

        public async Task<string> RefreshTokensAsync(string refreshToken, string email)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _options.UserPoolId,
                ClientId = _options.ClientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            };

            request.AuthParameters.Add("REFRESH_TOKEN", refreshToken);
            request.AuthParameters.Add("SECRET_HASH", CreatSecretHash(email));
            var response = await _cognitoService.AdminInitiateAuthAsync(request);
            return response.AuthenticationResult.AccessToken;
        }

        public async Task SendPasswordResetEmail(string email)
        {
            var forgotRequest = new ForgotPasswordRequest()
            {
                Username = email,
                SecretHash = CreatSecretHash(email),
                ClientId = _options.ClientId,
            };

            await _cognitoService.ForgotPasswordAsync(forgotRequest);
        }

        public async Task ResetPassword(string email, string code, string password)
        {
            var forgotRequest = new ConfirmForgotPasswordRequest()
            {
                Username = email,
                ConfirmationCode = code,
                Password = password,
                SecretHash = CreatSecretHash(email),
                ClientId = _options.ClientId,
            };

            await _cognitoService.ConfirmForgotPasswordAsync(forgotRequest);
        }

        public async Task SignOutAsync(string accessToken)
        {
            var request = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };
            await _cognitoService.GlobalSignOutAsync(request);
        }

        public async Task DeleteAsync(string id)
        {
            var request = new AdminDeleteUserRequest
            {
                UserPoolId = _options.UserPoolId,
                Username = id
            };
            await _cognitoService.AdminDeleteUserAsync(request);
        }

        private static TokenResponse CreateResponse(string accessToken, string refreshToken)
        {
            return new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        private string CreatSecretHash(string username)
        {
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.ClientSecret));
            var inputBytes = Encoding.UTF8.GetBytes(username + _options.ClientId);
            var hashBytes = hmac.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
