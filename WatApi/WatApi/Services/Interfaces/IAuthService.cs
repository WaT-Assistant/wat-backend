using WatApi.Models;

namespace WatApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> LoginAsync(WatApi.DTO.User.UserLoginDto dto);
    }
}
