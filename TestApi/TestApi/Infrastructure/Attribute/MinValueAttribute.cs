using System.ComponentModel.DataAnnotations;

namespace TestApi.Infrastructure.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MinValueAttribute : ValidationAttribute
    {
        private readonly int min;

        public MinValueAttribute(int min)
        {
            this.min = min;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (decimal.TryParse(value.ToString(), out decimal result))
            {
                if (result >= min)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Значение меньше допустимого.");
        }
    }
}