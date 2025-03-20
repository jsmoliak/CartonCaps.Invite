using CartonCaps.Invite.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text.Json;

namespace CartonCaps.Invite.API.Filters
{
    /// <summary>
    /// An exception filter that handles ReferralSourceNotFoundException exceptions, returning a 400 Bad Request response.
    /// </summary>
    public class ReferralSourceNotFoundExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ReferralSourceNotFoundExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSourceNotFoundExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log referral source not found exceptions.</param>
        public ReferralSourceNotFoundExceptionFilter(ILogger<ReferralSourceNotFoundExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception is thrown during the execution of an action.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ReferralSourceNotFoundException referralSourceNotFoundException)
            {
                _logger.LogWarning(context.Exception, "A request was made with a referral source that does not exist.");

                var error = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "No referrals exist with the provided referral code.",
                    status = 400,
                    errors = referralSourceNotFoundException.Message,
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
