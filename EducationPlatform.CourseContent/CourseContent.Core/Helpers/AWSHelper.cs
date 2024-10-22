using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CourseContent.Core.Helpers
{
    internal class AwsHelper(IAmazonS3 s3Client)
    {
        private readonly IAmazonS3 _s3Client = s3Client;

        public async Task<bool> PostObjectAsync(string bucketName, string objectName, IFormFile file)
        {
            PutObjectRequest request = CreatePutObjectRequest(bucketName, objectName, file);
            var response = await _s3Client.PutObjectAsync(request);
            return (response.HttpStatusCode == HttpStatusCode.OK ||
                    response.HttpStatusCode == HttpStatusCode.NoContent);
        }

        public async Task<bool> DeleteObjectAsync(string bucketName, string objectName)
        {
            DeleteObjectRequest deleteRequest = CreateDeleteObjectRequest(bucketName, objectName);
            var deleteResponse = await _s3Client.DeleteObjectAsync(deleteRequest);
            return (deleteResponse.HttpStatusCode == HttpStatusCode.OK ||
                   deleteResponse.HttpStatusCode == HttpStatusCode.NoContent);
        }

        public async Task<string> GeneratePresignedURLAsync(string bucketName, string objectKey, double duration)
        {
            GetPreSignedUrlRequest request = CreateGetPreSignedUrlRequest(bucketName, objectKey, duration);
            return await _s3Client.GetPreSignedURLAsync(request);
        }

        private static GetPreSignedUrlRequest CreateGetPreSignedUrlRequest(string bucketName,
            string objectKey, double duration)
        {
            return new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddHours(duration),
            };
        }

        private static DeleteObjectRequest CreateDeleteObjectRequest(string bucketName,
            string objectName)
        {
            return new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
            };
        }

        private static PutObjectRequest CreatePutObjectRequest(string bucketName,
            string objectName, IFormFile file)
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
