using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestApi.Attributes;

namespace TestApi.ViewModels
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
        public decimal Value { get; set; } 
    }
}