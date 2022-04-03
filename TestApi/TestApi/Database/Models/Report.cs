namespace TestApi.Database.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ToUserId { get; set; }
        public User ToUser { get; set; }
        public Guid PersonalAccountId { get; set; }
        public PersonalAccount PersonalAccount { get; set; }
        public Guid ToPersonalAccountId { get; set; }
        public PersonalAccount ToPersonalAccount { get; set; }
        public decimal Value { get; set; }
        public Currency Currency { get; set; }
        public DateTime DateTransfer { get; set; }
    }
}