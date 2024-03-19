using System.ComponentModel.DataAnnotations;

namespace EnrollHub.Application.Validators
{
    public class DateOfBirthNotGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateOfBirth = (DateTime)value;

                if (dateOfBirth > DateTime.Today)
                {
                    return new ValidationResult(Constants.DateOfBirthCannotBeInTheFutureMessage);
                }
            }

            return ValidationResult.Success;
        }
    }


    public class DateOfBirthShouldBeSixteenYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateOfBirth = (DateTime)value;

                if (dateOfBirth >= DateTime.Now.AddYears(-16))
                {
                    return new ValidationResult(Constants.MinimumEnrollmentAgeMessage);
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(Constants.MinimumEnrollmentAgeMessage);
            }

           
        }
    }
}
