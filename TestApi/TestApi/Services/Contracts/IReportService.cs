using TestApi.Database.Models;

namespace TestApi.Services.Contracts
{
    public interface IReportService
    {
        Task<Report> Create(Report report);
        Task<IEnumerable<Report>> GetReports(int userId);
    }
}
