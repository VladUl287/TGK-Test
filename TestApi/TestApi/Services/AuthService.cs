using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Dtos;
using TestApi.Infrastructure.Options;
using TestApi.Services.Contracts;

namespace TestApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext dbContext;
        private readonly PasswordOptions passwordOptions;
        private readonly AuthOptions authOptions;

        public AuthService(DatabaseContext dbContext, IOptions<PasswordOptions> passwordOptions, IOptions<AuthOptions> authOptions)
        {
            this.dbContext = dbContext;
            this.passwordOptions = passwordOptions.Value;
            this.authOptions = authOptions.Value;
        }

        public async Task<AuthSuccess?> Login(AuthModel login)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user is null)
            {
                return null;
            }

            var hashSecret = passwordOptions.HashSecret;
            var hashPassword = HashService.Hash(login.Password, hashSecret);

            if (user.Password != hashPassword)
            {
                return null;
            }

            var issuer = authOptions.Issuer;
            var audience = authOptions.Audience;
            var accessTokenKey = authOptions.AccessSecret;
            var refreshTokenKey = authOptions.RefreshSecret;
            var lifeTime = authOptions.LifeTime;

            var accessToken = JwtService.Generate(user, accessTokenKey, issuer, audience, DateTime.UtcNow.AddMinutes(lifeTime));
            var refreshToken = JwtService.Generate(user, refreshTokenKey, issuer, audience, DateTime.UtcNow.AddDays(lifeTime));

            await dbContext.Tokens.AddAsync(
                new Token
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken
                });
            await dbContext.SaveChangesAsync();

            return new AuthSuccess
            {
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthSuccess?> Register(AuthModel register)
        {
            var exists = await dbContext.Users.AnyAsync(e => e.Email == register.Email);

            if (exists)
            {
                return null;
            }

            var hashSecret = passwordOptions.HashSecret;
            var hashPassword = HashService.Hash(register.Password, hashSecret);

            var user = new User
            {
                Email = register.Email,
                Password = hashPassword
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return new AuthSuccess
            {
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

        public async Task<AuthSuccess?> Refresh(string token)
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

            var issuer = authOptions.Issuer;
            var audience = authOptions.Audience;
            var accessTokenKey = authOptions.AccessSecret;
            var refreshTokenKey = authOptions.RefreshSecret;
            var lifeTime = authOptions.LifeTime;

            var valid = JwtService.ValidateToken(dbToken.RefreshToken, refreshTokenKey, issuer, audience);

            if (!valid)
            {
                dbContext.Tokens.Remove(dbToken);
                await dbContext.SaveChangesAsync();

                return null;
            }

            var accessToken = JwtService.Generate(dbToken.User, accessTokenKey, issuer, audience, DateTime.UtcNow.AddMinutes(lifeTime));
            var refreshToken = JwtService.Generate(dbToken.User, refreshTokenKey, issuer, audience, DateTime.UtcNow.AddDays(lifeTime));

            dbToken.RefreshToken = refreshToken;
            dbContext.Tokens.Update(dbToken);
            await dbContext.SaveChangesAsync();

            return new AuthSuccess
            {
                Email = dbToken.User.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}