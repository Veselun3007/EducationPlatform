using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CourseContent.Infrastructure.Helpers
{
    public class FilesHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> AddFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    throw new ArgumentException("Invalid file.");
                }

                string object_name = Guid.NewGuid().ToString() + "_" + file.FileName;

                bool uploadSuccess = await AwsHelper
                    .PostObjectAsync(_configuration["AWSConfig:AccessKey"], _configuration["AWSConfig:SecretKey"],
                    _configuration["AWSConfig:BucketName"], object_name, file);

                return !uploadSuccess ? object_name : throw
                    new ApplicationException("File upload failed.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string name)
        {
            try
            {
                bool deleteSuccess = await AwsHelper
                .DeleteObjectAsync(_configuration["AWSConfig:SecretKey"],
                _configuration["AWSConfig:SecretKey"],
                _configuration["AWSConfig:BucketName"], name);

                return deleteSuccess ? true : throw
                    new ApplicationException("File delete failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
                throw;
            }
        }
    }
}

