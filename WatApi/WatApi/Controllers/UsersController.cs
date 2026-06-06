using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WatApi.DTO;
using WatApi.Models;
using WatApi.Data;
using Microsoft.EntityFrameworkCore;
using WatApi.Services.Interfaces;

namespace WatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WatApi.Services.Interfaces.IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            if (await _service.GetUserByEmailAsync(dto.EmailAddress) != null)
                return BadRequest("This email adress already exists!");

            User user = await _service.CreateUserAsync(dto);
            return StatusCode(201, new
            {
                user.Id,
                user.Email,
                user.FullName
            });
        }
                
    }
}
