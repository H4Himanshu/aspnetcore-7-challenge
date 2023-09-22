using System.ComponentModel.DataAnnotations;

namespace Challenge.Utilities
{
    public class DateOfBirthTypeValidationAttribute : ValidationAttribute
    {
        /// <summary>Validates the specified value with respect to the current validation attribute.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult">ValidationResult</see> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is DateOnly dateOfBirth)
                {
                    DateOnly dateOnly = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
                    DateTime convertedDateTime = dateOnly.ToDateTime(TimeOnly.Parse(TimeOnly.MinValue.ToString()));

                    DateTime minDate = new DateTime(1900, 1, 1);
                    DateTime maxDate = DateTime.Now;

                    if (convertedDateTime < minDate || convertedDateTime > maxDate)
                    {
                        return new ValidationResult("Invalid date of birth.");
                    }
                }
                else
                {
                    return new ValidationResult("Invalid date of birth data type.");
                }
            }

            return ValidationResult.Success; // Validation succeeded
        }
    }
}
