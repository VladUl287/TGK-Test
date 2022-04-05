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
        private readonly IJwtService jwtService;
        private readonly IConfiguration configuration;

        public AuthService(DatabaseContext dbContext, IJwtService tokenService, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.jwtService = tokenService;
            this.configuration = configuration;
        }

        public async Task<LoginSuccess?> Login(AuthModel login)
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

            var accessTokenKey = configuration.GetValue<string>("Token:AccessSecret");
            var refreshTokenKey = configuration.GetValue<string>("Token:RefreshSecret");
            var issuer = configuration.GetValue<string>("Token:Issuer");
            var audience = configuration.GetValue<string>("Token:Audience");
            var lifeTime = int.Parse(configuration.GetValue<string>("Token:LifeTime"));

            var accessToken = jwtService.Generate(user, accessTokenKey, issuer, audience, DateTime.UtcNow.AddMinutes(lifeTime));
            var refreshToken = jwtService.Generate(user, refreshTokenKey, issuer, audience, DateTime.UtcNow.AddDays(lifeTime));

            await dbContext.Tokens.AddAsync(new Token
            {
                UserId = user.Id,
                RefreshToken = refreshToken
            });
            await dbContext.SaveChangesAsync();

            return new LoginSuccess
            {
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
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

        public async Task Logout(string token)
        {
            var dbToken = await dbContext.Tokens
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RefreshToken == token);

            if (dbToken is null)
            {
                return;
            }

            dbContext.Tokens.Remove(dbToken);
            await dbContext.SaveChangesAsync();
        }

        public async Task<LoginSuccess?> Refresh(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var dbToken = await dbContext.Tokens
               .Include(e => e.User)
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.RefreshToken == token);

            if (dbToken is null)
            {
                return null;
            }

            var accessSecretKey = configuration.GetValue<string>("Token:AccessSecret");
            var refreshSecretKey = configuration.GetValue<string>("Token:RefreshSecret");
            var issuer = configuration.GetValue<string>("Token:Issuer");
            var audience = configuration.GetValue<string>("Token:Audience");
            var lifeTime = int.Parse(configuration.GetValue<string>("Token:LifeTime"));

            var valid = jwtService.ValidateToken(dbToken.RefreshToken, refreshSecretKey, issuer, audience);

            if (!valid)
            {
                await Logout(dbToken.RefreshToken);

                return null;
            }

            var accessToken = jwtService.Generate(dbToken.User, accessSecretKey, issuer, audience, DateTime.UtcNow.AddMinutes(lifeTime));
            var refreshToken = jwtService.Generate(dbToken.User, refreshSecretKey, issuer, audience, DateTime.UtcNow.AddDays(lifeTime));

            dbToken.RefreshToken = refreshToken;
            dbContext.Tokens.Update(dbToken);
            await dbContext.SaveChangesAsync();

            return new LoginSuccess 
            {
                Email = dbToken.User.Email,
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            };
        }
    }
}