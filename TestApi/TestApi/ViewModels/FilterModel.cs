namespace TestApi.ViewModels
{
    public class FilterModel
    {
        public Guid AccountNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyId { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
