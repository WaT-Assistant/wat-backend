namespace WatApi.Security
{
    public class RefreshTokenHasher
    {
        public static string HashToken(string token) => BCrypt.Net.BCrypt.HashPassword(token);
        public static bool Verify(string plainToken, string hashedToken) =>
            BCrypt.Net.BCrypt.Verify(plainToken, hashedToken);
    }
}
