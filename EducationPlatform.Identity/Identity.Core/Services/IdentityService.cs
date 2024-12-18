﻿using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CSharpFunctionalExtensions;
using Identity.Core.DTO.Responses;
using Identity.Core.Models;
using Identity.Domain.Config;
using Microsoft.Extensions.Options;

namespace Identity.Core.Services
{
    public class IdentityService(IAmazonCognitoIdentityProvider cognitoService, IOptions<AwsOptions> option)
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoService = cognitoService;
        private readonly AwsOptions _options = option.Value;

        public async Task<Result<string, Error>> SignUpAsync(string email, string password)
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
                return Result.Success<string, Error>(signUpResponse.UserSub);
            }
            catch (UsernameExistsException)
            {
                return Result.Failure<string, Error>(Errors.Identity.UsernameExist(email));
            }
        }

        public async Task<Result<TokenResponseModel, Error>> SignInAsync(string email, string password)
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
                return Result.Success<TokenResponseModel, Error>(CreateResponse(
                    response.AuthenticationResult.AccessToken,
                    response.AuthenticationResult.RefreshToken));
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<TokenResponseModel, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> ComfirmUserAsync(string email, string code)
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
                return Result.Success<string, Error>("Email confirmed");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
            catch (CodeMismatchException)
            {
                return Result.Failure<string, Error>(Errors.Identity.CodeMismatch());
            }
            catch (ExpiredCodeException)
            {
                return Result.Failure<string, Error>(Errors.Identity.ExpiredCode());
            }
        }

        public async Task<Result<string, Error>> RefreshTokensAsync(string refreshToken)
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
                return Result.Success<string, Error>(response.AuthenticationResult.AccessToken);
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> SendPasswordResetEmail(string email)
        {
            var forgotRequest = new ForgotPasswordRequest()
            {
                Username = email,
                ClientId = _options.ClientId,
            };
            try
            {
                await _cognitoService.ForgotPasswordAsync(forgotRequest);
                return Result.Success<string, Error>("A request for a password reset code has been sent");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> ResetPassword(string email, string code, string password)
        {
            var forgotRequest = new ConfirmForgotPasswordRequest()
            {
                Username = email,
                ConfirmationCode = code,
                Password = password,
                ClientId = _options.ClientId,
            };
            try
            {
                await _cognitoService.ConfirmForgotPasswordAsync(forgotRequest);
                return Result.Success<string, Error>("Password change successful");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }



        public async Task<Result<string, Error>> SignOutAsync(string accessToken)
        {
            try
            {
                var request = new GlobalSignOutRequest
                {
                    AccessToken = accessToken
                };
                await _cognitoService.GlobalSignOutAsync(request);
                return Result.Success<string, Error>("User successfully signed out");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> DeleteAsync(string id)
        {
            try
            {
                var request = new AdminDeleteUserRequest
                {
                    UserPoolId = _options.UserPoolId,
                    Username = id
                };
                var response = await _cognitoService.AdminDeleteUserAsync(request);
                return Result.Success<string, Error>("User successfully deleted");
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
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
