namespace TestApi.Database.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string DigitalСode { get; set; }
        public string LetterCode { get; set; }
        public string Name { get; set; }
        public char Sign { get; set; }
    }
}