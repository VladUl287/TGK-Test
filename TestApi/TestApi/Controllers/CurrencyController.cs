using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;

        public CurrencyController(DatabaseContext dbContext, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Currency>))]
        public async Task<IActionResult> GetAll()
        {
            if (!memoryCache.TryGetValue("currencies", out IEnumerable<Currency> currencies))
            {
                currencies = await dbContext.Currencies
                    .AsNoTracking()
                    .ToListAsync();

                if (currencies.Any())
                {
                    memoryCache.Set("currencies", currencies, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                }
            }

            return Ok(currencies);
        }
    }
}