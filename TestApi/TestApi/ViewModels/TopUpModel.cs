using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestApi.Infrastructure.Attribute;

namespace TestApi.ViewModels
{
    public class TopUpModel
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        public Guid AccountNumber { get; set; }

        [Required]
        [MinValue(0)]
        public decimal Value { get; set; }

        [Required]
        public int CurrencyId { get; set; }
    }
}