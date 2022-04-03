namespace TestApi.Database.Models
{
    public class PersonalAccount
    {
        public Guid Number { get; set; }
        public decimal Value { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}