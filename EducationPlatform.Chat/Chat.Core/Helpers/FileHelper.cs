using Identity.Domain.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CourseContent.Core.Helpers
{
    public class FileHelper(IOptions<AwsOptions> option)
    {
        private readonly AwsOptions _options = option.Value;

        public async Task<string> AddFileAsync(IFormFile file)
        {

            string objectName = Guid.NewGuid().ToString() + "_" + file.FileName;

            bool uploadSuccess = await AwsHelper.PostObjectAsync(_options.BucketName, objectName, file);

            return (uploadSuccess) ? objectName : "File upload failed.";
        }

        public async Task DeleteFileAsync(string name)
        {
            await AwsHelper.DeleteObjectAsync(_options.BucketName, name);
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await AwsHelper.GeneratePresignedURLAsync(_options.BucketName, fileName, 0.05);
        }
    }
}
