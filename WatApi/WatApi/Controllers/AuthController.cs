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
    public class AuthController(IUserService service, IAuthService authService) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly IAuthService _authService = authService;

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
            string token = await _authService.LoginAsync(dto);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(2)
            };
            Response.Cookies.Append("jwt", token, cookieOptions);
            return Ok(new { message = "Login successful" });
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            Response.Cookies.Delete("jwt", cookieOptions);
            return Ok(new { message = "Logout successful" });
        }
    }
}
