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

        public async Task<UserModel?> Login(AuthModel login)
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

        public async Task<UserModel?> Register(AuthModel register)
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
    }
}