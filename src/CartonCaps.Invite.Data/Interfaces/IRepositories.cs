using CartonCaps.Invite.Model.Entities;

namespace CartonCaps.Invite.Data.Interfaces
{
    /// <summary>
    /// Defines a contract for a repository of Referral entities.
    /// </summary>
    public interface IReferralsRepository : IRepositoryBase<Referral> { }

    /// <summary>
    /// Defines a contract for a repository of Redemption entities.
    /// </summary>
    public interface IRedemptionsRepository : IRepositoryBase<Redemption> { }

    /// <summary>
    /// Defines a contract for a repository of ReferralSource entities.
    /// </summary>
    public interface IReferralSourcesRepository : IRepositoryBase<ReferralSource>
    {
        /// <summary>
        /// Retrieves a ReferralSource entity by its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the ReferralSource.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<ReferralSource> GetByNameAsync(string name);
    }

    /// <summary>
    /// Defines a contract for a repository of User entities.
    /// </summary>
    public interface IUsersRepository : IRepositoryBase<User>
    {
        /// <summary>
        /// Retrieves a User entity by its authentication ID asynchronously.
        /// </summary>
        /// <param name="authId">The authentication ID of the User.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the User entity, or null if not found.</returns>
        public Task<User?> GetByAuthIdAsync(string authId);

        /// <summary>
        /// Retrieves a User entity by its referral code asynchronously.
        /// </summary>
        /// <param name="referralCode">The referral code of the User.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the User entity, or null if not found.</returns>
        public Task<User?> GetByReferralCodeAsync(string referralCode);
    }
}
