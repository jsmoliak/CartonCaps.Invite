
namespace CartonCaps.Invite.API.Infrastructure
{
    /// <summary>
    /// Represents an exception that occurs during profile management operations.
    /// </summary>
    [Serializable]
    public class ProfileManagementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManagementException"/> class.
        /// </summary>
        public ProfileManagementException()
        {
        }
    }
}