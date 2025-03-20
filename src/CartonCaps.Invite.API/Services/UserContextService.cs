using CartonCaps.Invite.API.Interfaces;

namespace CartonCaps.Invite.API.Services
{
    /// <summary>
    /// Service for retrieving user context information from the HTTP context.
    /// </summary>
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the authentication ID of the current user from the HTTP context.
        /// </summary>
        /// <returns>The authentication ID, or an empty string if not found.</returns>
        public string GetAuthId()
        {
            var authId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? String.Empty;
            return authId;
        }
    }
}
