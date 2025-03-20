using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text.Json;

namespace CartonCaps.Invite.API.Filters
{
    /// <summary>
    /// An exception filter that handles ArgumentException exceptions, returning a 400 Bad Request response.
    /// </summary>
    public class ArgumentExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ArgumentExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log argument exceptions.</param>
        public ArgumentExceptionFilter(ILogger<ArgumentExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception is thrown during the execution of an action.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException argumentException)
            {
                _logger.LogWarning(context.Exception, "An invalid request was detected.");

                var error = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors = argumentException.Message,
                    traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                };

                var json = JsonSerializer.Serialize(error);

                var result = new ContentResult
                {
                    StatusCode = 400,
                    ContentType = "application/json",
                    Content = json
                };
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
