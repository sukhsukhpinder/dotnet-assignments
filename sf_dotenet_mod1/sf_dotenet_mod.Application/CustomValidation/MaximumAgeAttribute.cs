using System.ComponentModel.DataAnnotations;

namespace sf_dotenet_mod.Application.CustomValidation
{
    public class MaximumAgeAttribute(int maximumAge) : ValidationAttribute
    {
        private readonly int _maximumAge = maximumAge;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = CalculateAge(dateOfBirth);
                if (age > _maximumAge)
                {
                    return new ValidationResult($"You cannot be older than {_maximumAge} years old.");
                }
            }

            return ValidationResult.Success;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
