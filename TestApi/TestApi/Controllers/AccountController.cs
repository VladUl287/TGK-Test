using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestApi.Database.Models;
using TestApi.Infrastructure.Exctension;
using TestApi.Services.Contracts;
using TestApi.ViewModels;

namespace TestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<PersonalAccount>>> Get()
        {
            var userId = User.GetLoggedInUserId<int>();

            return Ok(await accountService.Get(userId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountModel account)
        {
            account.UserId = User.GetLoggedInUserId<int>();
            var personalAccount = await accountService.CreateAccount(account);

            if (personalAccount is null)
            {
                return BadRequest("Ошибка создания лицевого счёта");
            }

            return Ok(personalAccount);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferModel transfer)
        {
            transfer.UserId = User.GetLoggedInUserId<int>();
            var transfered = await accountService.Transfer(transfer);

            if (!transfered)
            {
                return BadRequest("Ошибка перевода средств");
            }

            return Ok();
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] ConvertModel convertModel)
        {
            convertModel.UserId = User.GetLoggedInUserId<int>();
            var account = await accountService.Convert(convertModel);

            if (account is null)
            {
                return BadRequest("Ошибка конвертации счёта");
            }

            return Ok(account);
        }
    }
}
