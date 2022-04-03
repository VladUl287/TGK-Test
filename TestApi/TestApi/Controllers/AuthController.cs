using Microsoft.AspNetCore.Mvc;
using TestApi.Services.Contracts;
using TestApi.ViewModels;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromForm] AuthModel login)
        {
            var userModel = await authService.Login(login);

            if (userModel is null)
            {
                return BadRequest("Неверные email или пароль.");
            }

            Response.Cookies.Append("token", userModel.Token, new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(1))
            });

            return Ok(userModel);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] AuthModel register)
        {
            var user = await authService.Register(register);

            if (user is null)
            {
                return BadRequest("Пользователь с таким email уже существует.");
            }

            return Created("Register", new { user.Id });
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            return NoContent();
        }
    }
}