using WatApi.Models;

namespace WatApi.Services.Interfaces
{
    public record RefreshResult(
        string AccessToken,
        string RefreshToken
    );

    public interface ITokenService
    {
        public string GenerateJWT(User user);
        public Task<string> GenerateAndSaveRefreshToken(Guid userId);
        public Task RevokeAllRefreshTokensByIdAsync(Guid userId, string reason);
        public Task<RefreshResult> RefreshAsync(string refreshToken);
        public Task RevokeTokenAsync(string refreshToken);
    }
}
