using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestApi.ViewModels
{
    public class ConvertModel
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        public Guid AccountNumber { get; set; }

        [Required]
        public int CurrencyId { get; set; }
    }
}