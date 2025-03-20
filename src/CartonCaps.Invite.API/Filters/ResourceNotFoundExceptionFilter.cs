using CartonCaps.Invite.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CartonCaps.Invite.API.Filters
{
    /// <summary>
    /// An exception filter that handles ResourceNotFoundException exceptions, returning a 404 Not Found response.
    /// </summary>
    public class ResourceNotFoundExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ResourceNotFoundExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log resource not found exceptions.</param>
        public ResourceNotFoundExceptionFilter(ILogger<ResourceNotFoundExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception is thrown during the execution of an action.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/>.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ResourceNotFoundException)
            {
                _logger.LogWarning(context.Exception, "A request was made for a resource that does not exist.");

                var exception = context.Exception as ResourceNotFoundException;

                context.Result = new NotFoundResult();
                context.ExceptionHandled = true;
            }
        }
    }
}
