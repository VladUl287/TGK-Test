using TestApi.Database.Models;
using TestApi.ViewModels;

namespace TestApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<UserModel?> Login(LoginModel login);
        Task<UserModel?> Register(RegisterModel register);
        //Task<UserModel> Refresh(string token);
        //Task Logout(string token);
    }
}