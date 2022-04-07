using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestApi.Infrastructure.Attribute;

namespace TestApi.Dtos
{
    public class TransferModel
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        public Guid FromAccountNumber { get; set; }

        [Required]
        public Guid ToAccountNumber { get; set; }

        [Required]
        [MinValue(0)]
        public decimal Value { get; set; }
    }
}