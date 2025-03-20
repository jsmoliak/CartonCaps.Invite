using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.Exceptions;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Invite.API.Services
{
    /// <summary>
    /// Service for managing redemptions.
    /// </summary>
    public class RedemptionsService : IRedemptionsService
    {
        private readonly IUserContextService _userContextService;
        private readonly IUsersRepository _usersRepository;
        private readonly IRedemptionsRepository _redemptionsRepository;
        private readonly IReferralSourcesRepository _referralSourcesRepository;
        private readonly IProfileManagementApiClient _profileManagementApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedemptionsService"/> class.
        /// </summary>
        /// <param name="userContextService">The user context service.</param>
        /// <param name="usersRepository">The users repository.</param>
        /// <param name="redemptionsRepository">The redemptions repository.</param>
        /// <param name="referralSourcesRepository">The referral sources repository.</param>
        /// <param name="profileManagementApiClient">The profile management API client.</param>

        public RedemptionsService(IUserContextService userContextService, IUsersRepository usersRepository, IRedemptionsRepository redemptionsRepository, IReferralSourcesRepository referralSourcesRepository, IProfileManagementApiClient profileManagementApiClient)
        {
            _userContextService = userContextService;
            _usersRepository = usersRepository;
            _redemptionsRepository = redemptionsRepository;
            _referralSourcesRepository = referralSourcesRepository;
            _profileManagementApiClient = profileManagementApiClient;
        }

        /// <summary>
        /// Retrieves a redemption by its ID asynchronously.
        /// </summary>
        /// <param name="redemptionId">The ID of the redemption.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Redemption"/>, or throws an exception if not found.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the redemption is not found or the user is not authorized.</exception>
        public async Task<Redemption> GetRedemptionAsync(Guid redemptionId, CancellationToken cancellationToken)
        {
            var authId = _userContextService.GetAuthId();
            var redemption = await _redemptionsRepository.GetByIdAsync(redemptionId, cancellationToken);
            if (redemption is null || redemption.Redeemer.AuthId != authId)
                throw new ResourceNotFoundException();
            return redemption;
        }

        /// <summary>
        /// Retrieves a collection of redeemed referrals asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="RedeemedReferral"/>.</returns>
        public async Task<ICollection<RedeemedReferral>> ListRedeemedReferralsAsync()
        {
            var authId = _userContextService.GetAuthId();
            var referrer = await _usersRepository.GetByAuthIdAsync(authId);
            if (referrer is null) return [];

            ICollection<RedeemedReferral> referralRedemptions = new List<RedeemedReferral>();
            var redeemedReferrals = referrer.RedeemedReferrals.ToList();
            foreach (var redeemedReferral in redeemedReferrals)
            {
                var userProfile = await _profileManagementApiClient.GetUserProfileAsync(redeemedReferral.Redeemer.AuthId);
                referralRedemptions.Add(new RedeemedReferral(userProfile.FirstName, userProfile.LastName, userProfile.ReferralCode, redeemedReferral.RedeemedAt));
            }
            return referralRedemptions;
        }

        /// <summary>
        /// Adds a new redemption asynchronously.
        /// </summary>
        /// <param name="redemptionFromRequest">The redemption data transfer object.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created redemption.</returns>
        /// <exception cref="ReferralCodeNotFoundException">Thrown when the referral code is not found.</exception>
        /// <exception cref="UniqueRedemptionConstraintException">Thrown when a unique constraint is violated.</exception>
        /// <exception cref="DbUpdateException">Thrown when a database update fails.</exception>
        public async Task<Guid> AddRedemptionAsync(RedemptionRequest redemptionFromRequest, CancellationToken cancellationToken)
        {
            var authId = _userContextService.GetAuthId();
            var referralCode = redemptionFromRequest.ReferralCode;
            var referrer = await _usersRepository.GetByReferralCodeAsync(referralCode);
            if (referrer is null)
                throw new ReferralCodeNotFoundException();

            var redeemer = await _usersRepository.GetByAuthIdAsync(authId) ?? await _addRedeemer(authId, cancellationToken);
            var referralSource = await _referralSourcesRepository.GetByNameAsync(redemptionFromRequest.ReferralSource.ToString());
            var redemption = redeemer.AddRedemption(referrer.UserId, referrer.ReferralCode.ReferralCodeId, referralSource.ReferralSourceId);

            try
            {
                await _usersRepository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
                {
                    throw new UniqueRedemptionConstraintException();
                }
                throw;
            }
            return redemption.RedemptionId;
        }

        private async Task<User> _addRedeemer(string authId, CancellationToken cancellationToken)
        {
            User redeemer = new User(authId);
            await _usersRepository.AddAsync(redeemer, cancellationToken);
            return redeemer;
        }
    }
}
