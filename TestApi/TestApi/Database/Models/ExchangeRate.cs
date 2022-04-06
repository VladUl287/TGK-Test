namespace TestApi.Database.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int SecondCurrencyId { get; set; }
        public Currency SecondCurrency { get; set; }
        public decimal Rate { get; set; }

        public decimal Exchange(decimal sum)
        {
            return sum * Rate;
        }
    }
}