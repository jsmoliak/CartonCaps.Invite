namespace CartonCaps.Invite.Model.Interfaces
{
    /// <summary>
    /// Represents an entity that has an owner.
    /// </summary>
    public interface IOwnable
    {
        /// <summary>
        /// Gets the unique identifier of the owner.
        /// </summary>
        /// <returns>The owner's unique identifier.</returns>
        string GetOwnerId();
    }
}