using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;

namespace TestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public CurrencyController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Currency>))]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await dbContext.Currencies
                .AsNoTracking()
                .ToListAsync();

            return Ok(currencies);
        }
    }
}