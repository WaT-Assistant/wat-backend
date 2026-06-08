using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WatApi.Config;
using WatApi.DTO.User;
using WatApi.Models;
using WatApi.Security;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserService userService, IOptions<JwtSettings> jwtOptions)
        {
            _userService = userService;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _userService.GetUserByEmailAsync(dto.Email);
            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");
            return GenerateJWT(user);
        }

        private string GenerateJWT(User user)
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
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
