using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Services.Contracts;
using TestApi.ViewModels;

namespace TestApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext dbContext;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public AuthService(DatabaseContext dbContext, ITokenService tokenService, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        public async Task<UserModel?> Login(LoginModel login)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user is null)
            {
                return null;
            }

            var hashSecret = configuration.GetValue<string>("Hash:Secret");
            var hashPassword = HashService.Hash(login.Password, hashSecret);

            if (user.Password != hashPassword)
            {
                return null;
            }

            var key = configuration.GetValue<string>("Token:Secret");
            var issuer = configuration.GetValue<string>("Token:Issuer");
            var audience = configuration.GetValue<string>("Token:Audience");
            var lifeTime = int.Parse(configuration.GetValue<string>("Token:LifeTime"));

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Token = tokenService.Generate(user, key, issuer, audience, DateTime.Now.AddDays(lifeTime))
            };
        }

        public async Task<UserModel?> Register(RegisterModel register)
        {
            var exists = await dbContext.Users.AnyAsync(e => e.Email == register.Email);

            if (exists)
            {
                return null;
            }

            var key = configuration.GetValue<string>("Hash:Secret");
            var hashPassword = HashService.Hash(register.Password, key);

            var user = new User
            {
                Email = register.Email,
                Password = hashPassword
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        //    public Task Logout(string token)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task<UserModel?> Refresh(string token)
        //    {
        //        if (token is null)
        //        {
        //            return null;
        //        }

        //        //var dbToken = await dbContext.Tokens
        //        //   .AsNoTracking()
        //        //   .Select(x => new
        //        //   {
        //        //       x.UserId,
        //        //       x.User.Email,
        //        //       x.RefreshToken,
        //        //       RoleName = x.User.Role.Name
        //        //   })
        //        //   .FirstOrDefaultAsync(x => x.RefreshToken == token);

        //        //var valid = JwtService.ValidateToken(dbToken.RefreshToken, configuration.GetValue<string>("Secrets:JwtRefreshSecret"));

        //        //if (!valid)
        //        //{
        //        //    await dbContext.Database.ExecuteSqlInterpolatedAsync(
        //        //       $"DELETE FROM [Tokens] WHERE [RefreshToken] LIKE {dbToken.RefreshToken}");

        //        //    return null;
        //        //}

        //        //GenerateTokens(dbToken.UserId, dbToken.Email, dbToken.RoleName, out string accessToken, out string refreshToken);

        //        //await dbContext.Database.ExecuteSqlInterpolatedAsync(
        //        //    $"UPDATE [Tokens] SET [RefreshToken] = {dbToken.RefreshToken} WHERE [Id] = {dbToken.UserId}");

        //        //return new LoginSuccess { Token = accessToken, RefreshToken = refreshToken };
        //    }

        //    private void GenerateTokens(int id, string email, string role, out string accessToken, out string refreshToken)
        //    {
        //        accessToken = JwtService.Generate(
        //                        id,
        //                        email,
        //                        role,
        //                        configuration.GetValue<string>("Secrets:JwtAccessSecret"),
        //                        DateTime.Now.AddMinutes(15));
        //        refreshToken = JwtService.Generate(
        //                        id,
        //                        email,
        //                        role,
        //                        configuration.GetValue<string>("Secrets:JwtRefreshSecret"),
        //                        DateTime.Now.AddDays(15));
        //    }
    }
}
