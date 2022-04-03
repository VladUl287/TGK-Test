using TestApi.ViewModels;

namespace TestApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<UserModel?> Login(AuthModel login);

        Task<UserModel?> Register(AuthModel register);
    }
}