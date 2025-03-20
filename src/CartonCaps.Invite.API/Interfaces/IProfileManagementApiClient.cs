using CartonCaps.Invite.Model.ValueObjects;

namespace CartonCaps.Invite.API.Interfaces
{
    /// <summary>
    /// Defines a contract for a client that interacts with a profile management API.
    /// </summary>
    public interface IProfileManagementApiClient
    {
        /// <summary>
        /// Retrieves a user profile asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile is to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UserProfile"/>, or null if not found.</returns>
        public Task<UserProfile> GetUserProfileAsync(string userId);
    }
}