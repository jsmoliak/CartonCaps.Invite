using CartonCaps.Invite.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CartonCaps.Invite.API.Authorization
{
    /// <summary>
    /// Handles authorization based on ownership of a resource implementing IOwnable.
    /// </summary>
    public class OwnershipHandler : AuthorizationHandler<OwnershipRequirement, IOwnable>
    {
        /// <summary>
        /// Handles the authorization requirement for ownership of a resource.
        /// </summary>
        /// <param name="context">The authorization handler context.</param>
        /// <param name="requirement">The ownership requirement.</param>
        /// <param name="resource">The resource to be checked for ownership.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnershipRequirement requirement,
            IOwnable resource)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var authId = context.User.Identity.Name;
                if (resource.GetOwnerId() == authId) context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
