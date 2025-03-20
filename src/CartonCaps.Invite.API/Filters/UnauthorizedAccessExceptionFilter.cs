using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CartonCaps.Invite.API.Filters
{
    /// <summary>
    /// An exception filter that handles UnauthorizedAccessException exceptions, returning a 401 Unauthorized response.
    /// </summary>
    public class UnauthorizedAccessExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<UnauthorizedAccessExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedAccessExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log unauthorized access exceptions.</param>
        public UnauthorizedAccessExceptionFilter(ILogger<UnauthorizedAccessExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception is thrown during the execution of an action.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/>.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                _logger.LogWarning(context.Exception, "A request was made by a unauthorized party.");

                var exception = context.Exception as UnauthorizedAccessException;

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = exception?.Message
                };

                context.Result = new UnauthorizedObjectResult(problemDetails);
                context.ExceptionHandled = true;
            }
        }
    }
}
