using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CourseContent.Core.Helpers
{
    public class FilesHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _bucketName;

        public FilesHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = _configuration["AWSConfig:AccessKey"]!;
            _secretKey = _configuration["AWSConfig:SecretKey"]!;
            _bucketName = _configuration["AWSConfig:BucketName"]!;
        }

        public async Task<string> AddFileAsync(IFormFile file)
        {

            string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;

            bool uploadSuccess = await AwsHelper
                .PostObjectAsync(_accessKey, _secretKey, _bucketName, objectName, file);

            return (!uploadSuccess) ? objectName : "File upload failed.";
        }

        public async Task<bool> DeleteFileAsync(string name)
        {
            var deleteSuccess = await AwsHelper.DeleteObjectAsync(_accessKey, _secretKey, _bucketName, name);
            if (!deleteSuccess)
                return false;

            return true;
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await AwsHelper.GeneratePresignedURLAsync(_accessKey, _secretKey, _bucketName, fileName, 0.05);
        }
    }
}

