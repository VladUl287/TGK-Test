using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestApi.Infrastructure.Attribute;

namespace TestApi.Dtos
{
    public class AccountModel
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        [MinValue(0)]
        public decimal Value { get; set; }

        [Required]
        public int CurrencyId { get; set; }
    }
}