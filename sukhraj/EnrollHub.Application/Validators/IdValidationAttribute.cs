using System.ComponentModel.DataAnnotations;

namespace EnrollHub.Application.Validators
{
    public class IdValidationAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public IdValidationAttribute(int minValue)
        {
            _minValue = minValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (int)value < _minValue)
            {
                return new ValidationResult($"Please Select {validationContext.DisplayName}.");
            }

            return ValidationResult.Success;
        }
    }
}
