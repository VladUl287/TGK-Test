using TestApi.ViewModels;

namespace TestApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<LoginSuccess?> Login(AuthModel login);
        Task<UserModel?> Register(AuthModel register);
        Task Logout(string token);
        Task<LoginSuccess?> Refresh(string token);
    }
}