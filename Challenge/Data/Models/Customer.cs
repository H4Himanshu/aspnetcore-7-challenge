using System.ComponentModel.DataAnnotations;

namespace Challenge.Data.Models
{
    public class Customer
    {
        /// <summary>Gets or sets the customer identifier.</summary>
        /// <value>The customer identifier.</value>
        [Key]
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public required string FullName { get; set; }
        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>The date of birth.</value>
        public DateOnly DateOfBirth { get; set; }
        /// <summary>
        /// Gets or sets the profile image SVG.
        /// </summary>
        /// <value>The profile image SVG.</value>
        public required string ProfileImageSvg { get; set; }
    }
}
