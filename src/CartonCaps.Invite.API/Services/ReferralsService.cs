using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.Exceptions;

namespace CartonCaps.Invite.API.Services
{
    /// <summary>
    /// Service for managing referrals.
    /// </summary>
    public class ReferralsService : IReferralsService
    {
        private readonly IUserContextService _userContextService;
        private readonly IUsersRepository _usersRepository;
        private readonly IReferralsRepository _referralsRepository;
        private readonly IReferralSourcesRepository _referralSourcesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralsService"/> class.
        /// </summary>
        /// <param name="userContextService">The user context service.</param>
        /// <param name="usersRepository">The users repository.</param>
        /// <param name="referralsRepository">The referrals repository.</param>
        /// <param name="referralSourcesRepository">The referral sources repository.</param>
        public ReferralsService(IUserContextService userContextService, IUsersRepository usersRepository, IReferralsRepository referralsRepository, IReferralSourcesRepository referralSourcesRepository)
        {
            _userContextService = userContextService;
            _usersRepository = usersRepository;
            _referralsRepository = referralsRepository;
            _referralSourcesRepository = referralSourcesRepository;
        }

        /// <summary>
        /// Retrieves a read-only collection of referrals asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only collection of <see cref="Referral"/>.</returns>
        public async Task<IReadOnlyCollection<Referral>> ListReferralsAsync()
        {
            var authId = _userContextService.GetAuthId();
            var referrer = await _usersRepository.GetByAuthIdAsync(authId);
            if (referrer is null) return [];

            return referrer.Referrals;
        }

        /// <summary>
        /// Retrieves a referral by its ID asynchronously.
        /// </summary>
        /// <param name="referralId">The ID of the referral.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Referral"/>, or throws an exception if not found.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the referral is not found.</exception>
        public async Task<Referral> GetReferralAsync(Guid referralId, CancellationToken cancellationToken)
        {
            return await _referralsRepository.GetByIdAsync(referralId, cancellationToken) ?? throw new ResourceNotFoundException();
        }

        /// <summary>
        /// Adds a new referral asynchronously.
        /// </summary>
        /// <param name="referralFromRequest">The referral data transfer object.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created referral.</returns>
        /// <exception cref="ReferralCodeNotFoundException">Thrown when the referral code is not found.</exception>
        public async Task<Guid> AddReferralAsync(ReferralRequest referralFromRequest, CancellationToken cancellationToken)
        {
            var authId = _userContextService.GetAuthId();
            var referrer = await _usersRepository.GetByAuthIdAsync(authId) ?? await _addUserWithReferralCode(referralFromRequest, authId, cancellationToken);
            var referralSource = await _referralSourcesRepository.GetByNameAsync(referralFromRequest.ReferralSource.ToString());

            var referralCode = referrer.ReferralCode;
            if (referralCode.Code != referralFromRequest.ReferralCode)
                throw new ReferralCodeNotFoundException();

            var referral = referrer.AddReferral(referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _usersRepository.SaveChangesAsync(cancellationToken);
            return referral.ReferralId;
        }

        private async Task<User> _addUserWithReferralCode(ReferralRequest referralFromRequest, string authId, CancellationToken cancellationToken)
        {
            User referrer = new User(authId);
            referrer.ReferralCode = new ReferralCode(referralFromRequest.ReferralCode, referrer.UserId);
            await _usersRepository.AddAsync(referrer, cancellationToken);
            return referrer;
        }
    }
}
