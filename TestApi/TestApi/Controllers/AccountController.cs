using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApi.Database.Models;
using TestApi.Dtos;
using TestApi.Infrastructure.Exctension;
using TestApi.Services.Contracts;

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
                return BadRequest(Errors.FaildAccountCreate);
            }

            return Ok(personalAccount);
        }

        [HttpPost("topup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonalAccount))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUp([FromBody] TopUpModel topUpModel)
        {
            topUpModel.UserId = User.GetLoggedInUserId<int>();
            var account = await accountService.TopUp(topUpModel);

            if (account is null)
            {
                return BadRequest(Errors.FaildAccountTopUp);
            }

            return Ok(account);
        }

        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Transfer([FromBody] TransferModel transfer)
        {
            transfer.UserId = User.GetLoggedInUserId<int>();
            var transfered = await accountService.Transfer(transfer);

            if (!transfered)
            {
                return BadRequest(Errors.FaildTransfer);
            }

            return NoContent();
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
                return BadRequest(Errors.FaildAccountConvert);
            }

            return Ok(account);
        }
    }
}