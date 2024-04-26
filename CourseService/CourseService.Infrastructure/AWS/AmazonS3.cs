using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace FileAWS {
    public class AmazonS3 {
        private IAmazonS3 _client;
        public static readonly RegionEndpoint USEast1 = Amazon.RegionEndpoint.USEast1;
        private string _bucket = "educationplatform";

        public AmazonS3(string access_key, string secret_key, RegionEndpoint region_endpoint) {
            _client = new AmazonS3Client(access_key, secret_key,
                new AmazonS3Config { RegionEndpoint = region_endpoint });
        }

        public AmazonS3(IAmazonS3 client, string bucket) {
            _client = client;
            _bucket = bucket;
        }

        //видалити, для тестування
        public AmazonS3() { 
            _client = new AmazonS3Client("AKIA6GBMGKKRTD7XQ7W6", "rKGmDJysLQqLETz2/A0ivfL7ggK9wxefPB+JZK/A", 
                new AmazonS3Config { RegionEndpoint = USEast1 });
        }

        #region *** GetObject ***
        /// <summary>
        /// Повертає об'єкт з хмарного сховища Amazon S3
        /// </summary>
        /// <param name="access_key">Ключ доступу</param>
        /// <param name="secret_key">Секретний ключ</param>
        /// <param name="bucket_name">Назва корзини</param>
        /// <param name="object_name">Назва об'єкта</param>
        /// <returns>Об'єкт у вигляді масиву байт</returns>
        /// <exception cref="Exception">Об'єкт не знайдено</exception>
        public static async Task<byte[]> GetObjectAsync(string access_key, string secret_key, string bucket_name, string object_name, RegionEndpoint region = null) {
            AmazonS3Config config = new AmazonS3Config() { RegionEndpoint = region ?? Amazon.RegionEndpoint.USEast1 };
            using (var client = new AmazonS3Client(access_key, secret_key, config)) {
                return await GetObjectAsync(client, bucket_name, object_name);
            }
        }
        private static async Task<byte[]> GetObjectAsync(IAmazonS3 client, string bucket_name, string object_name) {
            GetObjectRequest request = new GetObjectRequest {
                BucketName = bucket_name,
                Key = object_name,
            };
            using GetObjectResponse response = await client.GetObjectAsync(request);
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        public async Task<byte[]> GetObjectAsync(string bucket_name, string object_name) {
            if (_client == null) throw new ArgumentNullException();
            return await GetObjectAsync(_client, bucket_name, object_name);
        }
        #endregion

        #region *** PostObject ***
        /// <summary>
        /// Додає об'єкт у хмарне сховище Amazon S3
        /// </summary>
        /// <param name="access_key">Ключ доступу</param>
        /// <param name="secret_key">Секретний ключ</param>
        /// <param name="bucket_name">Назва корзини</param>
        /// <param name="object_name">Назва об'єкта</param>
        /// <param name="file">Об'єкт, який необххідно додати до сховища</param>
        /// <returns>Результат виконання операції у вигляді булевого значення</returns>
        public static async Task<bool> PostObjectAsync(string access_key, string secret_key, string bucket_name, string object_name, IFormFile file, RegionEndpoint region = null) {
            AmazonS3Config config = new AmazonS3Config() { RegionEndpoint = region ?? Amazon.RegionEndpoint.USEast1 };
            using (var client = new AmazonS3Client(access_key, secret_key, config)) {
                return await PostObjectAsync(client, bucket_name, object_name, file);
            }
        }
        private static async Task<bool> PostObjectAsync(IAmazonS3 client, string bucket_name, string object_name, IFormFile file) {
            PutObjectRequest request = new PutObjectRequest() {
                BucketName = bucket_name,
                Key = object_name,
                InputStream = file.OpenReadStream(),
            };

            try {
                PutObjectResponse response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == HttpStatusCode.OK ||
                    response.HttpStatusCode == HttpStatusCode.NoContent) return true;
                return false;
            }
            catch (Exception) {
                return await DoesObjectExistAsync(client, bucket_name, object_name);
            }
        }
        public async Task<bool> PostObjectAsync(string bucket_name, string object_name, IFormFile file) {
            if (_client == null) throw new ArgumentNullException();
            return await PostObjectAsync(_client, bucket_name, object_name, file);
        }
        #endregion

        #region *** DeleteObject ***
        /// <summary>
        /// Видаляє з хмарного сховища Amazon S3
        /// </summary>
        /// <param name="access_key">Ключ доступу</param>
        /// <param name="secret_key">Секретний ключ</param>
        /// <param name="bucket_name">Назва корзини</param>
        /// <param name="object_name">Назва об'єкта</param>
        /// <returns>Результат виконання операції у вигляді булевого значення</returns>
        public static async Task<bool> DeleteObjectAsync(string access_key, string secret_key, string bucket_name, string object_name, RegionEndpoint region = null) {
            AmazonS3Config config = new AmazonS3Config() { RegionEndpoint = region ?? Amazon.RegionEndpoint.USEast1 };
            using (var client = new AmazonS3Client(access_key, secret_key, config)) {
                return await DeleteObjectAsync(client, bucket_name, object_name);
            }
        }
        private static async Task<bool> DeleteObjectAsync(IAmazonS3 client, string bucket_name, string object_name) {
            DeleteObjectRequest request = new DeleteObjectRequest() {
                BucketName = bucket_name,
                Key = object_name,
            };
            try {
                DeleteObjectResponse response = await client.DeleteObjectAsync(request);
            }
            catch (Exception) { }
            return !await DoesObjectExistAsync(client, bucket_name, object_name);
        }
        public async Task<bool> DeleteObjectAsync(string bucket_name, string object_name) {
            if (_client == null) throw new ArgumentNullException();
            return await DeleteObjectAsync(_client, bucket_name, object_name);
        }
        #endregion

        #region *** DoesObjectExist ***
        public static async Task<bool> DoesObjectExistAsync(string access_key, string secret_key, string bucket_name, string object_name, RegionEndpoint region = null) {
            AmazonS3Config config = new AmazonS3Config() { RegionEndpoint = region ?? Amazon.RegionEndpoint.USEast1 };
            using (var client = new AmazonS3Client(access_key, secret_key, config)) {
                return await DoesObjectExistAsync(client, bucket_name, object_name);
            }
        }
        public static async Task<bool> DoesObjectExistAsync(IAmazonS3 client, string bucket_name, string object_name) {
            try {
                GetObjectMetadataRequest metadataRequest = new GetObjectMetadataRequest {
                    BucketName = bucket_name,
                    Key = object_name
                };
                GetObjectMetadataResponse response = await client.GetObjectMetadataAsync(bucket_name, object_name);
                if (response == null) return false;
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound) {
                return false;
            }
        }
        public async Task<bool> DoesObjectExistAsync(string bucket_name, string object_name) {
            if (_client == null) throw new ArgumentNullException();
            return await DoesObjectExistAsync(_client, bucket_name, object_name);
        }
        #endregion

        #region *** GetObjectTemporaryUrl ***
        public static async Task<string> GetObjectTemporaryUrlAsync(string access_key, string secret_key, string bucket_name, string object_name, double expire = 1.0, RegionEndpoint region = null) {
            AmazonS3Config config = new AmazonS3Config() { RegionEndpoint = region ?? Amazon.RegionEndpoint.USEast1 };
            using (var client = new AmazonS3Client(access_key, secret_key, config)) {
                string url = await GetObjectTemporaryUrlAsync(client, bucket_name, object_name, expire = 1.0);
                return url;
            }
        }
        public static async Task<string> GetObjectTemporaryUrlAsync(IAmazonS3 client, string bucket_name, string object_name, double expire = 1.0) {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest {
                BucketName = bucket_name,
                Key = object_name,
                Expires = DateTime.Now.AddMinutes(expire),
                Protocol = Protocol.HTTPS,
                Verb = HttpVerb.GET,
            };
            string url = await client.GetPreSignedURLAsync(request);
            return url;
        }
        public async Task<string> GetObjectTemporaryUrlAsync(string bucket_name, string object_name) {
            if (_client == null) throw new ArgumentNullException();
            return await GetObjectTemporaryUrlAsync(_client, bucket_name, object_name);
        }


        public async Task<string> GetObjectTemporaryUrlAsync(string object_name) {
            //using var client = new AmazonS3Client();
            return await GetObjectTemporaryUrlAsync(_client, _bucket, object_name);
        }

        #endregion

    }
}