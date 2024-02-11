using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CourseContent.Core.Helpers
{
    internal class AwsHelper
    {
        private static readonly RegionEndpoint USEast1 = Amazon.RegionEndpoint.USEast1;

        public static async Task<bool> PostObjectAsync(string accessKey, string secretKey,
            string bucketName, string objectName, IFormFile file)
        {
            try
            {
                var config = new AmazonS3Config { RegionEndpoint = USEast1 };
                using var client = new AmazonS3Client(accessKey, secretKey, config);
                PutObjectRequest request = CreatePutObjectRequest(bucketName, objectName, file);

                var response = await client.PutObjectAsync(request);

                return (response.HttpStatusCode == HttpStatusCode.OK ||
                        response.HttpStatusCode == HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteObjectAsync(string accessKey, string secretKey,
            string bucketName, string objectName)
        {
            try
            {
                var config = new AmazonS3Config { RegionEndpoint = USEast1 };
                using var client = new AmazonS3Client(accessKey, secretKey, config);
                DeleteObjectRequest deleteRequest = CreateDeleteObjectRequest(bucketName, objectName);

                var deleteResponse = await client.DeleteObjectAsync(deleteRequest);

                if (deleteResponse.HttpStatusCode == HttpStatusCode.NoContent ||
                    deleteResponse.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Generates a temporary link to view a file from an AWS S3 storage
        /// </summary>
        /// <param name="accessKey">Vault access key</param>
        /// <param name="secretKey">Vault secret key</param>
        /// <param name="bucketName">Storage name (AWS S3 bucket`s name)</param>
        /// <param name="objectKey">Name of the desired file</param>
        /// <param name="duration">Duration of the temporary link (in hours)</param>
        /// <returns>A string value presigned URL</returns>
        public static async Task<string> GeneratePresignedURLAsync(string accessKey, string secretKey,
            string bucketName, string objectKey, double duration)
        {
            try
            {
                var config = new AmazonS3Config { RegionEndpoint = USEast1 };
                using var client = new AmazonS3Client(accessKey, secretKey, config);
                GetPreSignedUrlRequest request = CreateGetPreSignedUrlRequest(bucketName, objectKey, duration);

                return await client.GetPreSignedURLAsync(request);
            }
            catch
            {
                return "Fail generate operation";
            }
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
