using Challenge.Controllers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Challenge.Middleware
{
    public class ExceptionMiddleware
    {
        /// <summary>The next</summary>
        private readonly RequestDelegate _next;

        /// <summary>Initializes a new instance of the <see cref="T:Challenge.Middleware.ExceptionMiddleware" /> class.</summary>
        /// <param name="next">The next.</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>Invoke as an asynchronous operation.</summary>
        /// <param name="context">The context.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>Handle exception as an asynchronous operation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Default to Internal Server Error

            string message = "An error occurred.";

            if (exception is ApplicationException appException)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = appException.Message;
            }
            else if (exception is BadHttpRequestException badHttpRequestException)
            {
                // Handle BadHttpRequestException (e.g., invalid request content)
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                message = badHttpRequestException.Message;
            }
            else if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                // Handle other model validation errors (e.g., invalid request model)
                message = "Data Validation errors occurred.";
            }

            // Log the exception (you can customize your logging here)

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
