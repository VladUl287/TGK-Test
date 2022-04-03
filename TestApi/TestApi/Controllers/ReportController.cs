using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Infrastructure.Exctension;

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
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetLoggedInUserId<int>();

            var reports = await dbContext.Reports
                .Include(e => e.Currency)
                .Where(e => e.UserId == userId || e.ToUserId == userId)
                .AsNoTracking()
                .ToListAsync();

            //var fromReports = await dbContext.Reports
            //    .Include(e => e.User)
            //    .Include(e => e.ToUser)
            //    .Where(e => e.ToUserId == id && e.UserId != id)
            //    .AsNoTracking()
            //    .ToListAsync();

            //reports.AddRange(fromReports);

            return Ok(reports);
        }
    }
}