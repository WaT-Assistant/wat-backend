namespace WatApi.Security
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public static bool Verify(string plainPassword, string hashedPassword) => 
            BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
