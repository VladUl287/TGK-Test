using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonalAccount>))]
        public async Task<IActionResult> Get()
        {
            var userId = User.GetLoggedInUserId<int>();

            return Ok(await accountService.Get(userId));
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonalAccount))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AccountModel account)
        {
            account.UserId = User.GetLoggedInUserId<int>();
            var personalAccount = await accountService.Create(account);

            if (personalAccount is null)
            {
                return BadRequest("Ошибка создания лицевого счёта");
            }

            return Ok(personalAccount);
        }

        [HttpPost("topup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonalAccount))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Convert([FromBody] TopUpModel topUpModel)
        {
            topUpModel.UserId = User.GetLoggedInUserId<int>();
            var account = await accountService.TopUp(topUpModel);

            if (account is null)
            {
                return BadRequest("Ошибка пополнения счёта");
            }

            return Ok(account);
        }

        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonalAccount))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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