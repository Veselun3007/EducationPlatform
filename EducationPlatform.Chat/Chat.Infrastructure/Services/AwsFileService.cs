using Amazon.S3;
using Chat.Core.Interfaces.Infrastructure;
using Chat.Infrastructure.Helpers;
using Chat.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Chat.Infrastructure.Services
{
    public class AwsFileService : IAwsFileService
    {
        private readonly AwsOptions _options;
        private readonly AwsHelper _awsHelper;

        public AwsFileService(IOptions<AwsOptions> options, IAmazonS3 s3Client)
        {
            _options = options.Value;
            _awsHelper = new(s3Client);
        }
        public async Task<string> AddFileAsync(IFormFile file)
        {
            string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;
            bool uploadSuccess = await _awsHelper.PostObjectAsync(_options.BucketName, objectName, file);
            return uploadSuccess ? objectName : "File upload failed.";
        }

        public async Task DeleteFileAsync(string name)
        {
            await _awsHelper.DeleteObjectAsync(_options.BucketName, name);
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await _awsHelper.GeneratePresignedURLAsync(_options.BucketName, fileName, 0.05);
        }
    }
}
