using System.ComponentModel.DataAnnotations;

namespace TestApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MinMaxAttribute : ValidationAttribute
    {
        private readonly decimal min;
        private readonly decimal max;

        public MinMaxAttribute(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (decimal.TryParse(value.ToString(), out decimal result))
            {
                if (result >= min && result <= max)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Invalid value.");
        }
    }
}
