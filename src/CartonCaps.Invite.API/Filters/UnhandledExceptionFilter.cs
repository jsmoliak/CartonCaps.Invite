namespace CartonCaps.Invite.API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System.Net;

    /// <summary>
    /// A filter that handles unhandled exceptions in the API, logs them, and returns a generic 500 error response.
    /// </summary>
    public class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log exceptions.</param>
        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception occurs. Logs the exception and sets a generic 500 error response.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/> containing information about the exception and the request.</param>
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                _logger.LogError(context.Exception, "An unhandled exception was detected.");

                context.Result = new ObjectResult(new { error = "An unexpected error occurred." })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                context.ExceptionHandled = true;
            }
        }
    }
}