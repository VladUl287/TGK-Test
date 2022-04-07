using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class TopUpModel : AccountModel
    {
        [Required]
        public Guid AccountNumber { get; set; }
    }
}