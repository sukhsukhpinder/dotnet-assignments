using System.ComponentModel.DataAnnotations;

namespace sf_dotenet_mod.Application.CustomValidation
{
    public class MinimumAgeAttribute(int minimumAge) : ValidationAttribute
    {
        private readonly int _minimumAge = minimumAge;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = CalculateAge(dateOfBirth);
                if (age < _minimumAge)
                {
                    return new ValidationResult($"You must be at least {_minimumAge} years old.");
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
