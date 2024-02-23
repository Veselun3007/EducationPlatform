using Identity.Domain.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Identity.Core.Helpers
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

        public async Task<bool> DeleteFileAsync(string name)
        {
            var deleteSuccess = await AwsHelper.DeleteObjectAsync(_options.BucketName, name);
            if (!deleteSuccess)
                return false;

            return true;
        }

        public async Task<string> GetFileLink(string fileName)
        {
            return await AwsHelper.GeneratePresignedURLAsync(_options.BucketName, fileName, 0.05);
        }
    }
}
