using System.Security.Cryptography;
using System.Text;

namespace WatApi.Security
{
    public static class RefreshTokenHasher
    {
        public static string HashToken(string token) {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = SHA256.HashData(tokenBytes);
            return Convert.ToBase64String(hashBytes);
        }
        public static bool Verify(string plainToken, string hashedToken) 
            => HashToken(plainToken) == hashedToken;
    }
}
