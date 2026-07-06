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
        public Task<string> GenerateAndSaveRefreshToken(Guid userId, Guid deviceId);
        public Task RevokeAllRefreshTokensByIdAsync(Guid userId, string reason);
        public Task<RefreshResult> RefreshAsync(string refreshToken, Guid currentDeviceId);
        public Task RevokeTokenAsync(string refreshToken);
    }
}
