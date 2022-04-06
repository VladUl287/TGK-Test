using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Infrastructure.Exctension;
using TestApi.ViewModels;

namespace TestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public ReportController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Report>))]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filter)
        {
            var userId = User.GetLoggedInUserId<int>();

            var reports = dbContext.Reports
                .Include(e => e.Currency)
                .Where(e => e.UserId == userId);

            if (filter.AccountNumber != default)
            {
                reports = reports.Where(e => e.PersonalAccountId == filter.AccountNumber);
            }
            if (filter.StartDate != default)
            {
                reports = reports.Where(e => e.DateTransfer >= new DateTime(filter.StartDate.Date.Ticks, DateTimeKind.Utc));
            }
            if (filter.EndDate != default)
            {
                reports = reports.Where(e => e.DateTransfer <= new DateTime(filter.EndDate.Date.Ticks, DateTimeKind.Utc));
            }
            if (filter.CurrencyId != default)
            {
                reports = reports.Where(e => e.CurrencyId == filter.CurrencyId);
            }
            if (filter.Page != default && filter.Limit != default)
            {
                var skip = filter.Limit * (filter.Page - 1);

                reports = reports.Skip(skip).Take(filter.Limit);
            }

            return Ok(await reports.AsNoTracking().ToListAsync());
        }
    }
}