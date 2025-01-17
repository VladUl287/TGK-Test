﻿using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;
using TestApi.Services.Contracts;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private const string REFRESH_TOKEN = "refresh_token";

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccess))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromForm] AuthModel login)
        {
            var loginResult = await authService.Login(login);

            if (loginResult is null)
            {
                return BadRequest(Errors.NotCorrectEmailOrPassword);
            }

            Response.Cookies.Append(REFRESH_TOKEN, loginResult.RefreshToken, new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30))
            });

            return Ok(loginResult);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthSuccess))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] AuthModel register)
        {
            var auth = await authService.Register(register);

            if (auth is null)
            {
                return BadRequest(Errors.UserAlreadyExists);
            }

            return Created("Register", auth);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await authService.Logout(refreshToken);

                Response.Cookies.Delete(REFRESH_TOKEN);
            }

            return NoContent();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccess))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh()
        {
            var cookieToken = Request.Cookies[REFRESH_TOKEN];

            var result = await authService.Refresh(cookieToken);

            if (result is null)
            {
                return Unauthorized();
            }

            Response.Cookies.Append(REFRESH_TOKEN, result.RefreshToken, new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30))
            });

            return Ok(result);
        }
    }
}