using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatApi.Models;
using WatApi.Data;
using Microsoft.EntityFrameworkCore;
using WatApi.Services.Interfaces;
using WatApi.DTO.User;

namespace WatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService service, IAuthService authService,
        ITokenService tokenService) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            User user = await _service.CreateUserAsync(dto);
            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };

            return StatusCode(201, userResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var cookieDeviceId = Request.Cookies["deviceId"];
            if (string.IsNullOrWhiteSpace(cookieDeviceId) || !Guid.TryParse
                (cookieDeviceId, out Guid deviceId))
                deviceId = Guid.NewGuid();

            var user = await _authService.LoginAsync(dto);
            string accessToken = _tokenService.GenerateJWT(user);
            var refreshToken = await _tokenService.GenerateAndSaveRefreshToken(user.Id, deviceId);

            var cookieAccessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };
            var cookieRefreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            var cookieDeviceOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddYears(10)
            };

            Response.Cookies.Append("jwt", accessToken, cookieAccessOptions);
            Response.Cookies.Append("refreshToken", refreshToken, cookieRefreshOptions);
            Response.Cookies.Append("deviceId", deviceId.ToString(), cookieDeviceOptions);

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                try
                {
                    await _tokenService.RevokeTokenAsync(refreshToken);
                }
                catch (Exception)
                {
                    // Ignore exceptions during logout to ensure the user is
                    // logged out even if token revocation fails.
                }
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            Response.Cookies.Delete("jwt", cookieOptions);
            Response.Cookies.Delete("refreshToken", cookieOptions);
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Missing refresh token.");

            var cookieDeviceId = Request.Cookies["deviceId"];
            if (string.IsNullOrWhiteSpace(cookieDeviceId) || !Guid.TryParse
                (cookieDeviceId, out Guid deviceId))
                return Unauthorized("Missing or invalid device ID.");
            var result = await _tokenService.RefreshAsync(refreshToken, deviceId);

            var cookieAccessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var cookieRefreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("jwt", result.AccessToken, cookieAccessOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieRefreshOptions);

            return Ok(new { message = "Token refreshed" });
        }
    }
}
