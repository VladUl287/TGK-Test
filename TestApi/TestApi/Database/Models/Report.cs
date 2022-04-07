namespace TestApi.Database.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid PersonalAccountId { get; set; }
        public PersonalAccount PersonalAccount { get; set; }
        public decimal TransferValue { get; set; }
        public decimal AccountValue { get; set; }
        public int AccountCurrencyId { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public bool Credited { get; set; }
        public DateTime DateTransfer { get; set; }
    }
}