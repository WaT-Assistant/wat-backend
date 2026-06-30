using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
    public class AuthService(IUserService userService) : IAuthService
    {
        private readonly IUserService _userService = userService;

        public async Task<User> LoginAsync(UserLoginDto dto)
        {
            var user = await _userService.GetUserByEmailAsync(dto.Email);
            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");
            return user;
        }
    }
}
