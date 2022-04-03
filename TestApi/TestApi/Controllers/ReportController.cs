using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApi.Database;
using TestApi.Database.Models;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public ReportController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<IEnumerable<Report>> GetAll(int id)
        {
            var reports = await dbContext.Reports
                .Include(e => e.User)
                .Include(e => e.ToUser)
                .Where(e => e.UserId == id || e.ToUserId == id)
                .AsNoTracking()
                .ToListAsync();

            //var fromReports = await dbContext.Reports
            //    .Include(e => e.User)
            //    .Include(e => e.ToUser)
            //    .Where(e => e.ToUserId == id && e.UserId != id)
            //    .AsNoTracking()
            //    .ToListAsync();

            //reports.AddRange(fromReports);

            return reports;
        }
    }
}
