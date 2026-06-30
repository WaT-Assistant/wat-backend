using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WatApi.Config;
using WatApi.Models;
using Microsoft.Extensions.Options;
using WatApi.Services.Interfaces;
using WatApi.Data;
using System.Threading.Tasks;
using WatApi.Security;
using Microsoft.EntityFrameworkCore;

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

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = RefreshTokenHasher.HashToken(token),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            });
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshResult> RefreshAsync(string refreshToken)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => RefreshTokenHasher.Verify(refreshToken, rt.Token)) ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (!existingToken.IsActive)
            {
                if (existingToken.RevokedAt is not null)
                    await RevokeAllRefreshTokensByIdAsync(existingToken.UserId);
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            var user = await _context.Users.FindAsync(existingToken.UserId) ??
                throw new InvalidOperationException("User not found for the provided refresh token.");

            existingToken.RevokedAt = DateTime.UtcNow;
            var newRefreshToken = await GenerateRefreshToken(user.Id);
            existingToken.ReplacedByToken = RefreshTokenHasher.HashToken(newRefreshToken);
            await _context.SaveChangesAsync();

            var newAccessToken = GenerateJWT(user);
            return new RefreshResult(newAccessToken, newRefreshToken);
        }

        public async Task RevokeAllRefreshTokensByIdAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId && 
                rt.RevokedAt == null).ToListAsync();

            foreach (var token in tokens)
                token.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
