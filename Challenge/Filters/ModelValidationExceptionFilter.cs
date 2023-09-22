using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Challenge.Filters
{
    public class ModelValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidModelStateException modelStateException)
            {
                var errors = new Dictionary<string, string[]>();

                foreach (var error in modelStateException.Errors)
                {
                    var fieldName = error.MemberNames.FirstOrDefault() ?? "UnknownField";
                    var errorMessage = error.ErrorMessage;

                    if (!errors.ContainsKey(fieldName))
                    {
                        errors[fieldName] = new string[] { errorMessage };
                    }
                    else
                    {
                        errors[fieldName] = errors[fieldName].Append(errorMessage).ToArray();
                    }
                }

                var result = new ObjectResult(new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors
                })
                {
                    StatusCode = 400
                };

                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
