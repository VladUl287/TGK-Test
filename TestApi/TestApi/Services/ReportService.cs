using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Dtos;
using TestApi.Services.Contracts;

namespace TestApi.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseContext dbContext;

        public ReportService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Report>> GetReports(int userId, FilterModel filter)
        {
            var reports = dbContext.Reports
                .Include(e => e.Currency)
                .Where(e => e.UserId == userId);

            reports = SetFilters(filter, reports);

            return await reports
                .AsNoTracking()
                .OrderBy(e => e.DateTransfer)
                .ToListAsync();
        }

        private static IQueryable<Report> SetFilters(FilterModel filter, IQueryable<Report> reports)
        {
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
                reports = reports.Where(e => e.AccountCurrencyId == filter.CurrencyId);
            }
            if (filter.Page != default && filter.Limit != default)
            {
                var skip = filter.Limit * (filter.Page - 1);

                reports = reports.Skip(skip).Take(filter.Limit);
            }

            return reports;
        }
    }
}