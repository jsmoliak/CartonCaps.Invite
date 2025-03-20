using CartonCaps.Invite.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CartonCaps.Invite.API.Filters
{
    /// <summary>
    /// An exception filter that handles UniqueRedemptionConstraintException exceptions, returning a 409 Conflict response.
    /// </summary>
    public class UniqueRedemptionExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<UniqueRedemptionExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueRedemptionExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log unique redemption exceptions.</param>
        public UniqueRedemptionExceptionFilter(ILogger<UniqueRedemptionExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception is thrown during the execution of an action.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/>.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UniqueRedemptionConstraintException)
            {
                _logger.LogWarning(context.Exception, "A request was made by a party for a referral code that was already redeemed.");

                var exception = context.Exception as UniqueRedemptionConstraintException;

                context.Result = new ConflictResult();
                context.ExceptionHandled = true;
            }
        }
    }
}
