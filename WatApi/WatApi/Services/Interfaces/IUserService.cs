using WatApi.Data;
using WatApi.Models;
using WatApi.DTO;

namespace WatApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User>? GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(UserRegistrationDto dto);
    }
}
