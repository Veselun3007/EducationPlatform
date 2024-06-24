using Amazon.S3;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace CourseContent.Tests.Base
{
    public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>,
      IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly EducationPlatformContext _dbContext;
        protected IAmazonS3 _s3Client;

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EducationPlatformContext>();
            _s3Client = _scope.ServiceProvider.GetRequiredService<IAmazonS3>();

            var dataSetup = new DataSetup(_dbContext);
            dataSetup.AddBaseData();
        }
       
        public void Dispose()
        {
            _scope?.Dispose();
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}