﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CourseContent.Infrastructure.Helpers
{
    internal static class AWSHelper
    {
        public static readonly RegionEndpoint USEast1 = Amazon.RegionEndpoint.USEast1;

        public static async Task<byte[]> GetObjectAsync(string? access_key, string? secret_key,
            string? bucket_name, string? object_name)
        {
            var config = new AmazonS3Config { RegionEndpoint = USEast1 };

            try
            {
                using var client = new AmazonS3Client(access_key, secret_key, config);
                var request = new GetObjectRequest
                {
                    BucketName = bucket_name,
                    Key = object_name
                };

                using var response = await client.GetObjectAsync(request);
                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> PostObjectAsync(string? access_key, string? secret_key, 
            string? bucket_name, string? object_name, IFormFile file)
        {
            AmazonS3Config config;
            try
            {
                config = new AmazonS3Config() { RegionEndpoint = USEast1 };
            }
            catch (Exception)
            {
                return false;
            }

            using var client = new AmazonS3Client(access_key, secret_key, config);
            PutObjectRequest request = new()
            {
                BucketName = bucket_name,
                Key = object_name,
                InputStream = file.OpenReadStream(),
            };
            try
            {
                PutObjectResponse response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK ||
                    response.HttpStatusCode != HttpStatusCode.NoContent) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteObjectAsync(string? access_key, string? secret_key, 
            string? bucket_name, string? object_name)
        {
            AmazonS3Config config;
            try
            {
                config = new AmazonS3Config() { RegionEndpoint = USEast1 };
            }
            catch (Exception)
            {
                return false;
            }

            using var client = new AmazonS3Client(access_key, secret_key, config);
            DeleteObjectRequest request = new()
            {
                BucketName = bucket_name,
                Key = object_name,
            };
            try
            {
                DeleteObjectResponse response = await client.DeleteObjectAsync(request);
                return response.DeleteMarker != null;
            }
            catch (Exception)
            {
                GetObjectRequest get_request = new()
                {
                    BucketName = bucket_name,
                    Key = object_name,
                };
                try
                {
                    using GetObjectResponse get_response = await client.GetObjectAsync(get_request);
                    return false;
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

    }
}