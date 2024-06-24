using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Identity.Core.Services;
using Identity.Domain.Config;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Identity.Tests.UnitTests
{
    public class IdentityServiceTests(ITestOutputHelper output)
    {
        private readonly ITestOutputHelper _output = output;
        protected Mock<IAmazonCognitoIdentityProvider> _cognitoService = new();
        protected Mock<IOptions<AwsOptions>> _awsOptionsMock = new();

        [Theory]
        [InlineData("test@example.com", "@6sfhgs#D")]
        public async Task SignUpAsync_ReturnsSuccess(string email, string password)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });
            _cognitoService.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SignUpResponse { UserSub = "UserSub123456" });

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.SignUpAsync(email, password);

            Assert.True(result.IsSuccess);
            _output.WriteLine("UserSub" + result.Value);
        }

        [Theory]
        [InlineData("test@example.com", "@6sfhgs#D")]
        public async Task SignUpAsync_ReturnsFailure(string email, string password)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UsernameExistsException("User already exists"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.SignUpAsync(email, password);

            Assert.False(result.IsSuccess);
            Assert.Equal($"User with email {email} already exists", result.Error.Message);
            _output.WriteLine("Error message:\t" + result.Error.Message);
        }

        [Theory]
        [InlineData("fedyshenbohdan@gmail.com", "@4sfhgs#D")]
        public async Task SignInAsync_ReturnsSuccess(string email, string password)
        {
            _cognitoService.Setup(x => x.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), default))
                .ReturnsAsync(new InitiateAuthResponse
                {
                    AuthenticationResult = new AuthenticationResultType
                    {
                        AccessToken = "access_token",
                        RefreshToken = "refresh_token"
                    }
                });

            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.SignInAsync(email, password);

            Assert.True(result.IsSuccess);
            _output.WriteLine("Tokens:\nAccessToken:\t" + result.Value.AccessToken +
                "\nRefreshToken:\t" + result.Value.RefreshToken);
        }

        [Theory]
        [InlineData("test@example.com", "123456")]
        public async Task ConfirmUserAsync_ReturnsSuccess_ForValidConfirmation(string email, string code)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), default));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.ComfirmUserAsync(email, code);

            Assert.True(result.IsSuccess);
            Assert.Equal("Email confirmed", result.Value);
            _output.WriteLine(result.Value);
        }

        [Theory]
        [InlineData("test@example.com", "123456")]
        public async Task ConfirmUserAsync_ReturnsFailure_ForNotAuthorizedException(string email, string code)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException("No object with this key was found"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.ComfirmUserAsync(email, code);

            Assert.False(result.IsSuccess);
            Assert.Equal("No object with this key was found", result.Error.Message);
            _output.WriteLine("Error message: \t" + result.Error.Message);
        }

        [Theory]
        [InlineData("test@example.com", "123456")]
        public async Task ConfirmUserAsync_ReturnsFailure_ForCodeMismatchException(string email, string code)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), default))
                .ThrowsAsync(new CodeMismatchException("Code is not correct"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.ComfirmUserAsync(email, code);

            Assert.False(result.IsSuccess);
            Assert.Equal("Code is not correct", result.Error.Message);
            _output.WriteLine("Error message: \t" + result.Error.Message);
        }

        [Theory]
        [InlineData("test@example.com", "123456")]
        public async Task ConfirmUserAsync_ReturnsFailure_ForExpiredCodeException(string email, string code)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), default))
                .ThrowsAsync(new ExpiredCodeException("Code is expired"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.ComfirmUserAsync(email, code);

            Assert.False(result.IsSuccess);
            Assert.Equal("Code is expired", result.Error.Message);
            _output.WriteLine("Error message: \t" + result.Error.Message);
        }


        [Theory]
        [InlineData("fedyshenb@gmail.com", "@4sfhgs#D")]
        public async Task SignInAsync_ReturnsFailure(string email, string password)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId" });

            _cognitoService.Setup(x => x.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException("No object with this key was found"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.SignInAsync(email, password);

            Assert.Equal("No object with this key was found", result.Error.Message);
            _output.WriteLine("Error message:\n\t" + result.Error.Message);
        }

        [Theory]
        [InlineData("refresh_token")]
        public async Task RefreshTokensAsync_ReturnsSuccess(string refresh_token)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId", UserPoolId = "userPoolId" });

            _cognitoService.Setup(x => x.AdminInitiateAuthAsync(It.IsAny<AdminInitiateAuthRequest>(), default))
                .ReturnsAsync(new AdminInitiateAuthResponse
                {
                    AuthenticationResult = new AuthenticationResultType
                    {
                        AccessToken = "new_access_token"
                    }
                });

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.RefreshTokensAsync(refresh_token);

            Assert.True(result.IsSuccess);
            _output.WriteLine("New access token:\n\t" + result.Value);
        }

        [Theory]
        [InlineData("refresh_token")]
        public async Task RefreshTokensAsync_ReturnsFailure(string refresh_token)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { ClientId = "clientId", UserPoolId = "userPoolId" });

            _cognitoService.Setup(x => x.AdminInitiateAuthAsync(It.IsAny<AdminInitiateAuthRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException("No object with this key was found"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.RefreshTokensAsync(refresh_token);

            Assert.Equal("No object with this key was found", result.Error.Message);
            _output.WriteLine("Error message:\n\t" + result.Error.Message);
        }

        [Theory]
        [InlineData("access_token")]
        public async Task SignOutAsync_ReturnsSuccess(string accessToken)
        {
            _cognitoService.Setup(x => x.GlobalSignOutAsync(
                It.IsAny<GlobalSignOutRequest>(), default));

            var identityService = new IdentityService(_cognitoService.Object,
                _awsOptionsMock.Object);
            var result = await identityService.SignOutAsync(accessToken);

            Assert.True(result.IsSuccess);
            Assert.Equal("User successfully signed out", result.Value);
            _output.WriteLine("Value:\n\t" + result.Value);
        }

        [Theory]
        [InlineData("invalid_access_token")]
        public async Task SignOutAsync_ReturnsFailure(string accessToken)
        {
            _cognitoService.Setup(x => x.GlobalSignOutAsync(It.IsAny<GlobalSignOutRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException("No object with this key was found"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.SignOutAsync(accessToken);

            Assert.False(result.IsSuccess);
            Assert.Equal("No object with this key was found", result.Error.Message);
            _output.WriteLine("Error message:\n\t" + result.Error.Message);
        }

        [Theory]
        [InlineData("user_id")]
        public async Task DeleteAsync_ReturnsSuccess(string id)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { UserPoolId = "userPoolId" });

            _cognitoService.Setup(x => x.AdminDeleteUserAsync(
                It.IsAny<AdminDeleteUserRequest>(), default))
                .ReturnsAsync(new AdminDeleteUserResponse());

            var identityService = new IdentityService(_cognitoService.Object,
                _awsOptionsMock.Object);
            var result = await identityService.DeleteAsync(id);

            Assert.True(result.IsSuccess);
            Assert.Equal("User successfully deleted", result.Value);
            _output.WriteLine("Value:\n\t" + result.Value);
        }

        [Theory]
        [InlineData("invalid_user_id")]
        public async Task DeleteAsync_ReturnsFailure(string id)
        {
            _awsOptionsMock.Setup(x => x.Value)
                .Returns(new AwsOptions { UserPoolId = "userPoolId" });

            _cognitoService.Setup(x => x.AdminDeleteUserAsync(
                It.IsAny<AdminDeleteUserRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException("No object with this key was found"));

            var identityService = new IdentityService(_cognitoService.Object, _awsOptionsMock.Object);
            var result = await identityService.DeleteAsync(id);

            Assert.False(result.IsSuccess);
            Assert.Equal("No object with this key was found", result.Error.Message);
            _output.WriteLine("Error message:\n\t" + result.Error.Message);
        }
    }
}
