using Amazon.S3;
using CourseContent.Core.Interfaces;
using CourseContent.Infrastructure.Helpers;
using CourseContent.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CourseContent.Infrastructure.Services
{
    internal class AwsFileService : IAwsFileService
    {
        private readonly AwsOptions _options;
        private readonly AwsHelper _awsHelper;
        private const float PresignedUrlExpiryHours = 0.5f;

        public AwsFileService(IOptions<AwsOptions> options, IAmazonS3 s3Client)
        {
            _options = options.Value;
            _awsHelper = new(s3Client);
        }

        public async Task<string?> AddFileAsync(IFormFile file)
        {
            string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;
            bool uploadSuccess = await _awsHelper.PostObjectAsync(_options.BucketName, objectName, file);
            return uploadSuccess ? objectName : null;
        }

        public async Task DeleteFileAsync(string name)
        {
            await _awsHelper.DeleteObjectAsync(_options.BucketName, name);
        }

        public async Task<string?> GetFileLink(string fileName)
        {
            return await _awsHelper.GeneratePresignedUrlAsync(_options.BucketName, fileName, PresignedUrlExpiryHours);
        }
    }
}