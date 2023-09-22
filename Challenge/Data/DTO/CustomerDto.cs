using Challenge.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Data.DTO
{
    /// <summary>Class CustomerDto.</summary>
    public class CustomerDto
    {
        /// <summary>Gets or sets the full name.</summary>
        /// <value>The full name.</value>
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name must be between 1 and 100 characters.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>The date of birth.</value>
        [DateOfBirthTypeValidation]
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
    }
}
