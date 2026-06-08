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
    public class AuthController : ControllerBase
    {
        private readonly WatApi.Services.Interfaces.IUserService _service;
        private readonly WatApi.Services.Interfaces.IAuthService _authService;

        public AuthController(IUserService service, IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

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
                return Ok(new { Token = token });
        }
    }
}
