namespace WatApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(WatApi.DTO.User.UserLoginDto dto);
    }
}
