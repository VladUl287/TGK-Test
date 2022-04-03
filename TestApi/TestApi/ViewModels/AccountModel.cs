using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestApi.Database.Models;

namespace TestApi.ViewModels
{
    public class AccountModel
    {
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}