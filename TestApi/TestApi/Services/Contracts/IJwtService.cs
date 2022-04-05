using TestApi.Database.Models;

namespace TestApi.Services.Contracts
{
    public interface IJwtService
    {
        string Generate(User user, string key, string issuer, string audience, DateTime expires);
        bool ValidateToken(string token, string key, string issuer, string audience);
    }
}