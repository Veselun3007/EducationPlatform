using Amazon.Runtime;
using Amazon.S3;
using CourseContent.Infrastructure.Context;
using CourseContent.Web;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.LocalStack;
using Testcontainers.PostgreSql;

namespace CourseContent.Tests.Base
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly ushort LocalStackPort = 4566;

        private readonly PostgreSqlContainer _pgContainer;
        private readonly LocalStackContainer _localStack;

        private AmazonS3Client? _s3Client;

        public TestWebApplicationFactory()
        {
            _pgContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine")
                .WithDatabase("educationdb")
                .WithUsername("veselun")
                .WithPassword("postgres")               
                .WithPortBinding(Random.Shared.Next(10000, 65535), 5432)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(5432))                
                .Build();

            _localStack = new LocalStackBuilder()
                .WithImage("localstack/localstack:latest")
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(request => request
                        .ForPath("/_localstack/health")
                        .ForPort(LocalStackPort))).Build();

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<EducationPlatformContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContextPool<EducationPlatformContext>(options =>
                    options.UseNpgsql(_pgContainer.GetConnectionString()));


                services.AddScoped<IAmazonS3, AmazonS3Client>(provider =>
                {
                    AmazonS3Config config = new()
                    {
                        ServiceURL = $"http://localhost:{LocalStackPort}",
                        UseHttp = true,
                        ForcePathStyle = true,
                        AuthenticationRegion = "us-east-1"
                    };
                    AWSCredentials creds = new BasicAWSCredentials("key", "secret");
                    _s3Client = new(creds, config);


                    return _s3Client;
                });

            });
        }
       
        public async Task InitializeAsync()
        {
            await _localStack.StartAsync();
            await _pgContainer.StartAsync();
        }


        public new async Task DisposeAsync()
        {
            await _localStack.DisposeAsync();
            await _pgContainer.StopAsync();
        }
    }
}