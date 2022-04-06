namespace TestApi.Database.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string DigitalСode { get; set; } = string.Empty;
        public string LetterCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public char Sign { get; set; }
    }
}