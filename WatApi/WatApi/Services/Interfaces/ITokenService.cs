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
        public Task<string> GenerateRefreshToken(Guid userId);
        public Task RevokeAllRefreshTokensByIdAsync(Guid userId);
        public Task<RefreshResult> RefreshAsync(string refreshToken);
    }
}
