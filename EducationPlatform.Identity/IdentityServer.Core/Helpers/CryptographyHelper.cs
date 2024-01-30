using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.Core.Helpers
{
    public class CryptographyHelper()
    {
        public static string Hash(string text, string salt)
        {
            return Convert.ToHexString(SHA256.HashData(UTF8Encoding.UTF8.GetBytes(text + salt)));
        }

        public static bool VerifyPassword(string salt, string passwordToCheck, string password)
        {
            return Hash(passwordToCheck, salt) == password;
        }

        public static string GenerateSalt()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }
    }
}
