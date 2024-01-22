using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.Web.Helpers
{
    public class CryptographyHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string Hash (string text, string salt)
        {
            var sha256 = SHA256.Create();
            return Convert.ToHexString(sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(text + salt)));
        }

        public string GenerateSalt()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(int.Parse(_configuration["Password:SaltSize"])));
        }


        public bool VerifyPassword(string salt, string passwordToCheck, string password) { 
            return this.Hash(passwordToCheck, salt) == password;
        }
    }
}
