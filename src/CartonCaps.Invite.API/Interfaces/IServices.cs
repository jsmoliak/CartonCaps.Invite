using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;

namespace CartonCaps.Invite.API.Interfaces
{
    /// <summary>
    /// Defines a contract for managing referrals.
    /// </summary>
    public interface IReferralsService
    {
        /// <summary>
        /// Retrieves a read-only collection of referrals asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only collection of <see cref="Referral"/>.</returns>
        public Task<IReadOnlyCollection<Referral>> ListReferralsAsync();

        /// <summary>
        /// Retrieves a referral by its ID asynchronously.
        /// </summary>
        /// <param name="referralId">The ID of the referral.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Referral"/>, or null if not found.</returns>
        public Task<Referral> GetReferralAsync(Guid referralId, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new referral asynchronously.
        /// </summary>
        /// <param name="referralDto">The referral data transfer object.</param>
        /// <param name="cancellation">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created referral.</returns>
        public Task<Guid> AddReferralAsync(ReferralRequest referralDto, CancellationToken cancellation);

    }

    /// <summary>
    /// Defines a contract for managing redemptions.
    /// </summary>
    public interface IRedemptionsService
    {
        /// <summary>
        /// Retrieves a redemption by its ID asynchronously.
        /// </summary>
        /// <param name="redemptionId">The ID of the redemption.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Redemption"/>, or null if not found.</returns>
        public Task<Redemption> GetRedemptionAsync(Guid redemptionId, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new redemption asynchronously.
        /// </summary>
        /// <param name="redemptionFromDto">The redemption data transfer object.</param>
        /// <param name="cancellation">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created redemption.</returns>
        public Task<Guid> AddRedemptionAsync(RedemptionRequest redemptionFromDto, CancellationToken cancellation);

        /// <summary>
        /// Retrieves a collection of redeemed referrals asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="RedeemedReferral"/>.</returns>
        public Task<ICollection<RedeemedReferral>> ListRedeemedReferralsAsync();
    }

    /// <summary>
    /// Defines a contract for retrieving user context information.
    /// </summary>
    public interface IUserContextService
    {
        /// <summary>
        /// Retrieves the authentication ID of the current user.
        /// </summary>
        /// <returns>The authentication ID.</returns>
        string GetAuthId();
    }
}
