using System.ComponentModel.DataAnnotations;

namespace Challenge.Filters
{
    public class InvalidModelStateException : Exception
    {
        public IEnumerable<ValidationResult> Errors { get; }

        public InvalidModelStateException(IEnumerable<ValidationResult> errors)
        {
            Errors = errors;
        }
    }
}
