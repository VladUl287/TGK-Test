using TestApi.Dtos;

namespace TestApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthSuccess?> Login(AuthModel login);

        Task<AuthSuccess?> Register(AuthModel register);

        Task Logout(string token);

        Task<AuthSuccess?> Refresh(string token);
    }
}