using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Filters
{
    /// <summary>Class GlobalExceptionFilter.
    /// Implements the <see cref="IExceptionFilter" /></summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>The logger</summary>
        private readonly ILogger<GlobalExceptionFilter> _logger;

        /// <summary>Initializes a new instance of the <see cref="T:Challenge.Utilities.GlobalExceptionFilter" /> class.</summary>
        /// <param name="logger">The logger.</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>Called after an action has thrown an <see cref="T:System.Exception">Exception</see>.</summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext">ExceptionContext</see>.</param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            var result = new ObjectResult(new
            {
                StatusCode = 500,
                Message = "An internal server error occurred.",
                Details = context.Exception.Message,
            })
            {
                StatusCode = 500, // Internal Server Error
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
