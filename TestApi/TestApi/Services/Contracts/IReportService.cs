using TestApi.Database.Models;
using TestApi.Dtos;

namespace TestApi.Services.Contracts
{
    public interface IReportService
    {
        public Task<IEnumerable<Report>> GetReports(int userId, FilterModel filter);
    }
}