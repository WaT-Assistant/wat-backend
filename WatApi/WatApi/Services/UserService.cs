using Microsoft.EntityFrameworkCore;
using WatApi.Data;
using WatApi.DTO.User;
using WatApi.Models;
using WatApi.Security;

namespace WatApi.Services
{
    public class UserService : WatApi.Services.Interfaces.IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(UserRegistrationDto dto)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Email = dto.EmailAddress,
                FullName = dto.FullName,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
