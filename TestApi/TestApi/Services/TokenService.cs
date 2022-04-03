using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApi.Database.Models;
using TestApi.Services.Contracts;

namespace TestApi.Services
{
    public class TokenService : ITokenService
    {
        public string Generate(User user, string key, string issuer, string audience, DateTime expires)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentialsAccess = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role)
            };

            var securityToken = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: credentialsAccess);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}