using Amazon.CognitoIdentityProvider;
using Identity.Core.DTO.Requests;
using Identity.Core.Helpers;
using Identity.Core.Services;
using Identity.Domain.Config;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;

namespace Identity.Tests.UnitTests
{
    public class UserServiceTests(ITestOutputHelper output) : IAsyncLifetime
    {
        private readonly ITestOutputHelper _output = output;
        protected Mock<IAmazonCognitoIdentityProvider> _cognitoService = new();
        protected Mock<IOptions<AwsOptions>> _awsOptionsMock = new();
        private EducationPlatformContext _dbContext;
        private UserService _userService;

        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();
            var connectionString = _postgres.GetConnectionString();
            var dbContextOptions = new DbContextOptionsBuilder<EducationPlatformContext>()
                 .UseNpgsql(connectionString)
                 .Options;

            _dbContext = new EducationPlatformContext(dbContextOptions);
            _dbContext.Database.EnsureCreated();

            var dbOperation = new DbOperation(_dbContext);
            var identityOperation = new IdentityService(new Mock<IAmazonCognitoIdentityProvider>().Object, new Mock<IOptions<AwsOptions>>().Object);
            var filesHelper = new FileHelper(new Mock<IOptions<AwsOptions>>().Object);
            _userService = new UserService(dbOperation, identityOperation, filesHelper);
        }
        public Task DisposeAsync()
        {
            return _postgres.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {            
            var userDto = new UserDTO
            {
                UserName = "Test User",
                Password = "#2deffDsf3",
                Email = "test@gmail.com",
                UserImage = null
            };
            var id = "test_id";

            var result = await _userService.AddAsync(userDto, id);

            Assert.True(result.IsSuccess);
            _output.WriteLine("User:\n\t Username: " + result.Value.UserName +
                                   "\n\t    Email: " + result.Value.Email);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserToDatabase()
        {

            var userDto = new UserUpdateDTO
            {
                UserName = "Not Test User",
                Email = "test@gmail.com",
                UserImage = null
            };
            var id = "test_id";

            var result = await _userService.UpdateAsync(userDto, id);

            Assert.True(result.IsSuccess);
            _output.WriteLine("User:\n\t Username: " + result.Value.UserName +
                                   "\n\t    Email: " + result.Value.Email);
        }
    }
}
