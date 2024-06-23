﻿using Amazon.Runtime;
using Amazon.S3;
using CourseContent.Infrastructure.Context;
using CourseContent.Tests.Config;
using CourseContent.Tests.Model;
using CourseContent.Web;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.LocalStack;
using Testcontainers.PostgreSql;

namespace CourseContent.Tests.Base
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly PostgreSqlContainer _pgContainer;
        private readonly LocalStackContainer _localStack;

        private static string ToAbsolute(string path) => Path.GetFullPath(path);

        public TestWebApplicationFactory()
        {
            _pgContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine")
                .WithDatabase("educationdb")
                .WithUsername("veselun")
                .WithPassword("postgres")
                .WithPortBinding(Random.Shared.Next(10000, 20000), 5432)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(5432))
                .Build();

            _localStack = new LocalStackBuilder()
                .WithImage("localstack/localstack")
                .WithCleanUp(true)
                .WithPortBinding(Random.Shared.Next(4000, 5000), Setup.LocalStackPort)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(request => request
                    .ForPath("/_localstack/health")
                    .ForPort(Setup.LocalStackPort)))
                .WithBindMount(ToAbsolute("./localstack/aws-seed-data"), "/etc/localstack/init/ready.d", AccessMode.ReadOnly)
                .WithBindMount(ToAbsolute("./localstack/aws-seed-data/scripts"), "/scripts", AccessMode.ReadOnly)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.IntegrationTest.json", false);
                });

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<EducationPlatformContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContextPool<EducationPlatformContext>(options =>
                    options.UseNpgsql(_pgContainer.GetConnectionString()));

                /*services.AddScoped<IAmazonS3>(provider =>
                {

                    AmazonS3Config config = new()
                    {
                        ServiceURL = $"http://localhost/{Setup.LocalStackPort}",
                        UseHttp = true,
                        ForcePathStyle = true
                    };
                    AWSCredentials creds = new BasicAWSCredentials("dummy-access-key", "dummy-secret-key");
                    _s3Client = new(creds, config);

                    return _s3Client;
                });*/

                services.AddSingleton<IAmazonS3>(sc =>
                {
                    var configuration = sc.GetRequiredService<IConfiguration>();
                    var awsConfiguration = configuration.GetSection("AWS").Get<AwsConfiguration>();

                    if (awsConfiguration?.ServiceURL is null)
                    {
                        return new AmazonS3Client();
                    }
                    else
                    {
                        return AwsS3ClientFactory.CreateAwsS3Client(
                            awsConfiguration.ServiceURL!,
                            awsConfiguration.Region!, awsConfiguration.ForcePathStyle!,
                            awsConfiguration.AwsAccessKey!, awsConfiguration.AwsSecretKey!);
                    }
                });
            });
        }

        public async Task InitializeAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            await _localStack.StartAsync(cts.Token);
            await _pgContainer.StartAsync(cts.Token);
        }

        public new async Task DisposeAsync()
        {
            await _localStack.DisposeAsync();
            await _pgContainer.StopAsync();
        }
    }
}