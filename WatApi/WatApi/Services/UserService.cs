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
            if (await GetUserByEmailAsync(dto.EmailAddress) != null)
                throw new InvalidOperationException("This email adress already exists!");
            User user = new()
            {
                Id = Guid.NewGuid(),
                Email = dto.EmailAddress,
                FullName = dto.FullName,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
