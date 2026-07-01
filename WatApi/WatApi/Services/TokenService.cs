using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WatApi.Config;
using WatApi.Data;
using WatApi.Models;
using WatApi.Security;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class TokenService(IOptions<JwtSettings> jwtOptions, AppDbContext context) : ITokenService
    {
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        private readonly AppDbContext _context = context;

        public string GenerateJWT(User user)
        {
            var key = _jwtSettings.Key ?? throw new InvalidOperationException("JWT key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string TrackNewRawRefreshToken(Guid userId)
        {
            var selector = GenerateBase64UrlToken(16);
            var secret = GenerateBase64UrlToken(32);
            var rawRefreshToken = $"{selector}.{secret}";

            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Selector = selector,
                Token = RefreshTokenHasher.HashToken(secret),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            });

            return rawRefreshToken;
        }

        public async Task<string> GenerateAndSaveRefreshToken(Guid userId)
        {
            var rawRefreshToken = TrackNewRawRefreshToken(userId);
            await _context.SaveChangesAsync();
            return rawRefreshToken;
        }

        public async Task<RefreshResult> RefreshAsync(string refreshToken)
        {
            var (selector, secret) = ParseRefreshToken(refreshToken);

            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Selector == selector)
                ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (!RefreshTokenHasher.Verify(secret, existingToken.Token))
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (!existingToken.IsActive)
            {
                if (existingToken.RevokedAt is not null)
                {
                    await RevokeAllRefreshTokensByIdAsync
                        (existingToken.UserId, "Suspicious activity, possible security breach");
                }

                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _context.Users.FindAsync(existingToken.UserId)
                ?? throw new InvalidOperationException("User not found for the provided refresh token.");

            existingToken.RevokedAt = DateTime.UtcNow;

            var newRefreshToken =  TrackNewRawRefreshToken(user.Id);
            var (newSelector, _) = ParseRefreshToken(newRefreshToken);
            existingToken.ReplacedByToken = newSelector;

            await _context.SaveChangesAsync();

            var newAccessToken = GenerateJWT(user);
            return new RefreshResult(newAccessToken, newRefreshToken);
        }

        public async Task RevokeAllRefreshTokensByIdAsync(Guid userId, string reason)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();
                foreach (var token in tokens)
                {
                    token.RevokedAt = DateTime.UtcNow;
                    token.RevokeReason = reason;
                }

            await _context.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var (selector, secret) = ParseRefreshToken(refreshToken);
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Selector == selector)
                ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (existingToken is not null && existingToken.IsActive &&
                RefreshTokenHasher.Verify(secret, existingToken.Token))
            {
                existingToken.RevokedAt = DateTime.UtcNow;
                existingToken.RevokeReason = "User requested revocation (logout).";
                await _context.SaveChangesAsync();
            }
        }

        private static (string Selector, string Secret) ParseRefreshToken(string token)
        {
            var parts = token.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            return (parts[0], parts[1]);
        }

        private static string GenerateBase64UrlToken(int byteLength)
        {
            var bytes = RandomNumberGenerator.GetBytes(byteLength);
            return Base64UrlEncoder.Encode(bytes);
        }
    }
}