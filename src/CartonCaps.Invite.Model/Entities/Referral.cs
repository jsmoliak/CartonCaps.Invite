using CartonCaps.Invite.Model.Interfaces;

namespace CartonCaps.Invite.Model.Entities
{
    /// <summary>
    /// Represents a referral.
    /// </summary>
    public class Referral : IOwnable
    {
        /// <summary>
        /// Gets the unique identifier of the referral.
        /// </summary>
        public Guid ReferralId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the referrer (user who provided the referral).
        /// </summary>
        public Guid ReferrerId { get; private set; }

        private User? _referrer;

        /// <summary>
        /// Gets or sets the referrer user.
        /// </summary>
        public virtual User Referrer
        {
            set => _referrer = value;
            get => _referrer ?? throw new InvalidOperationException("Uninitialized property " + nameof(Referrer));
        }

        /// <summary>
        /// Gets the unique identifier of the referral code used for the referral.
        /// </summary>
        public Guid ReferralCodeId { get; private set; }

        private ReferralCode? _referralCode;

        /// <summary>
        /// Gets or sets the referral code used for the referral.
        /// </summary>
        public virtual ReferralCode ReferralCode
        {
            set => _referralCode = value;
            get => _referralCode ?? throw new InvalidOperationException("Uninitialized property " + nameof(ReferralCode));
        }

        /// <summary>
        /// Gets the unique identifier of the referral source.
        /// </summary>
        public int ReferralSourceId { get; private set; }

        private ReferralSource? _referralSource;

        /// <summary>
        /// Gets or sets the referral source.
        /// </summary>
        public virtual ReferralSource ReferralSource
        {
            set => _referralSource = value;
            get => _referralSource ?? throw new InvalidOperationException("Uninitialized property " + nameof(ReferralSource));
        }

        /// <summary>
        /// Gets the date and time when the referral was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="Referral"/> class.
        /// </summary>
        /// <param name="referrerId">The unique identifier of the referrer.</param>
        /// <param name="referralCodeId">The unique identifier of the referral code.</param>
        /// <param name="referralSourceId">The unique identifier of the referral source.</param>
        public Referral(Guid referrerId, Guid referralCodeId, int referralSourceId)
        {
            ReferrerId = referrerId;
            ReferralCodeId = referralCodeId;
            ReferralSourceId = referralSourceId;
        }

        /// <summary>
        /// Gets the owner ID of the referral.
        /// </summary>
        /// <returns>The owner ID, which is the authentication ID of the referrer.</returns>
        public string GetOwnerId()
        {
            return Referrer.AuthId;
        }
    }
}