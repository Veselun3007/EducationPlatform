using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CSharpFunctionalExtensions;
using Identity.Core.DTO.Token;
using Identity.Domain.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Core.Services
{
    public class IdentityOperation(IAmazonCognitoIdentityProvider cognitoService, ILogger<IdentityOperation> logger, IOptions<AwsOptions> option)
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoService = cognitoService;
        private readonly ILogger<IdentityOperation> _logger = logger;
        private readonly AwsOptions _options = option.Value;

        public async Task<Result<string>> SignUpAsync(string email, string password)
        {
            var signUpRequest = new SignUpRequest()
            {
                ClientId = _options.ClientId,
                Username = email,
                Password = password
            };
            try
            {
                var signUpResponse = await _cognitoService.SignUpAsync(signUpRequest);
                return Result.Success(signUpResponse.UserSub);
            }
            catch (UsernameExistsException)
            {
                return Result.Failure<string>($"User with email {email} already exists");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Sign Up: {ErrorMessage}", ex.Message);
                return Result.Failure<string>($"Oops, something went wrong");
            }
        }

        public async Task<Result<TokenResponseModel>> SignInAsync(string email, string password)
        {
            Dictionary<string, string> authParams = new()
            {
                {"USERNAME", email},
                {"PASSWORD", password}
            };

            InitiateAuthRequest request = new()
            {
                ClientId = _options.ClientId,
                AuthParameters = authParams,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            };
            try
            {
                var response = await _cognitoService.InitiateAuthAsync(request);
                return Result.Success(CreateResponse(
                    response.AuthenticationResult.AccessToken,
                    response.AuthenticationResult.RefreshToken));
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<TokenResponseModel>("User wasn`t found");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Sign In: {ErrorMessage}", ex.Message);
                return Result.Failure<TokenResponseModel>($"Oops, something went wrong");
            }
        }

        public async Task<Result<string>> ComfirmUserAsync(string email, string code)
        {
            var comfirmationRequest = new ConfirmSignUpRequest()
            {
                Username = email,
                ConfirmationCode = code,
                ClientId = _options.ClientId,
            };
            try
            {
                await _cognitoService.ConfirmSignUpAsync(comfirmationRequest);
                return Result.Success("Email confirmed");
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<string>($"User with email {email} not found");
            }
            catch (CodeMismatchException)
            {
                return Result.Failure<string>($"Code isn`t correct");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Confirm Sign Up: {ErrorMessage}", ex.Message);
                return Result.Failure<string>($"Oops, something went wrong");
            }
        }

        public async Task<Result<TokenResponseModel>> RefreshTokensAsync(string refreshToken)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _options.UserPoolId,
                ClientId = _options.ClientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH
            };

            request.AuthParameters.Add("REFRESH_TOKEN", refreshToken);
            try
            {
                var response = await _cognitoService.AdminInitiateAuthAsync(request);
                return Result.Success(CreateResponse(
                    response.AuthenticationResult.AccessToken,
                    response.AuthenticationResult.RefreshToken));
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<TokenResponseModel>($"User wasn`t authorize");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Refresh Tokens: {ErrorMessage}", ex.Message);
                return Result.Failure<TokenResponseModel>($"Oops, something went wrong");
            }
        }

        public async Task<Result> SignOutAsync(string accessToken)
        {
            var request = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };
            try
            {
                var response = await _cognitoService.GlobalSignOutAsync(request);
                return Result.Success("User successfully signed out");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure($"User wasn`t authorize");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Sign Out: {ErrorMessage}", ex.Message);
                return Result.Failure($"Oops, something went wrong");
            }
        }

        public async Task<Result> DeleteAsync(string accessToken)
        {
            var request = new DeleteUserRequest
            {
                AccessToken = accessToken
            };
            try
            {
                var response = await _cognitoService.DeleteUserAsync(request);
                return Result.Success("User successfully deleted");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure($"User wasn`t authorize");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Delete: {ErrorMessage}", ex.Message);
                return Result.Failure($"Oops, something went wrong");
            }
        }

        private static TokenResponseModel CreateResponse(string accessToken,
            string refreshToken)
        {
            return new TokenResponseModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
