using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StudentResult.Application.Interfaces;
using StudentResult.Infrastructure.Helpers;
using StudentResult.Infrastructure.Options;

namespace StudentResult.Infrastructure.Services
{
    internal class AwsFileService : IAwsFileService
    {
        private readonly AwsOptions _options;
        private readonly AwsHelper _awsHelper;
        private const float PresignedUrlExpiryHours = 1;

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

        public async Task<bool> DeleteFileAsync(string name)
        {
            return await _awsHelper.DeleteObjectAsync(_options.BucketName, name);
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await _awsHelper.GeneratePresignedUrlAsync(_options.BucketName, fileName, PresignedUrlExpiryHours);
        }
    }
}
