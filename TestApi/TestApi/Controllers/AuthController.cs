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
        public async Task<IActionResult> Login([FromForm] LoginModel login)
        {
            var userModel = await authService.Login(login);

            if (userModel is null)
            {
                return BadRequest("Неверные email или пароль.");
            }

            Response.Cookies.Append("token", userModel.Token);

            return Ok(userModel);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel register)
        {
            var user = await authService.Register(register);

            if (user is null)
            {
                return BadRequest(new { error = "Пользователь с таким email уже существует." });
            }

            return Created("Register", new { user.Id });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            return NoContent();
        }
    }
}