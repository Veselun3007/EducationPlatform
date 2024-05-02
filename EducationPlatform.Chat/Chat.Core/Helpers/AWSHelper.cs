using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace EPChat.Core.Helpers
{
    internal class AwsHelper
    {
        public static async Task<bool> PostObjectAsync(string bucketName, string objectName, IFormFile file)
        {
            using var client = new AmazonS3Client();
            PutObjectRequest request = CreatePutObjectRequest(bucketName, objectName, file);

            var response = await client.PutObjectAsync(request);

            return (response.HttpStatusCode == HttpStatusCode.OK ||
                    response.HttpStatusCode == HttpStatusCode.NoContent);
        }

        public static async Task<bool> DeleteObjectAsync(string bucketName, string objectName)
        {
            using var client = new AmazonS3Client();
            DeleteObjectRequest deleteRequest = CreateDeleteObjectRequest(bucketName, objectName);

            var deleteResponse = await client.DeleteObjectAsync(deleteRequest);

            if (deleteResponse.HttpStatusCode == HttpStatusCode.NoContent ||
                deleteResponse.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return true;
            }
            return false;
        }

        public static async Task<string> GeneratePresignedURLAsync(string bucketName, string objectKey, double duration)
        {
            using var client = new AmazonS3Client();
            GetPreSignedUrlRequest request = CreateGetPreSignedUrlRequest(bucketName, objectKey, duration);

            return await client.GetPreSignedURLAsync(request);
        }

        private static GetPreSignedUrlRequest CreateGetPreSignedUrlRequest(string bucketName, string objectKey, double duration)
        {
            return new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddHours(duration),
            };
        }

        private static DeleteObjectRequest CreateDeleteObjectRequest(string bucketName, string objectName)
        {
            return new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
            };
        }

        private static PutObjectRequest CreatePutObjectRequest(string bucketName, string objectName, IFormFile file)
        {
            return new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = file.OpenReadStream(),
            };
        }
    }
}
