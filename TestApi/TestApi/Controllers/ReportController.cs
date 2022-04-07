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
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Report>))]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filter)
        {
            var userId = User.GetLoggedInUserId<int>();

            return Ok(await reportService.GetReports(userId, filter));
        }
    }
}