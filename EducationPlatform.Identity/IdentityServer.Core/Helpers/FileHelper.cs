using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Core.Helpers
{
    public class FileHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _buckenName;

        public FileHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = _configuration["AWSConfig:AccessKey"]!;
            _secretKey = _configuration["AWSConfig:SecretKey"]!;
            _buckenName = _configuration["AWSConfig:BucketName"]!;
        }

        public async Task<string> AddFileAsync(IFormFile file)
        {

            string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;

            bool uploadSuccess = await AwsHelper
                .PostObjectAsync(_accessKey, _secretKey, _buckenName, objectName, file);

            return (uploadSuccess) ? objectName : "File upload failed.";
        }

        public async Task<bool> DeleteFileAsync(string name)
        {
            var deleteSuccess = await AwsHelper.DeleteObjectAsync(_accessKey, _secretKey, _buckenName, name);
            if (!deleteSuccess)
                return false;

            return true;
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await AwsHelper.GeneratePresignedURLAsync(_accessKey, _secretKey, _buckenName, fileName, 0.05);
        }
    }
}
