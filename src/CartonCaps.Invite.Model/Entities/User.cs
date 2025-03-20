namespace CartonCaps.Invite.Model.Entities
{
    /// <summary>
    /// Represents a user in the invite system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the authentication ID of the user.
        /// </summary>
        public string AuthId { get; private set; }

        private readonly List<Referral> _referrals = [];

        /// <summary>
        /// Gets a read-only collection of referrals made by the user.
        /// </summary>
        public virtual IReadOnlyCollection<Referral> Referrals => _referrals.AsReadOnly();

        private readonly List<Redemption> _redemptions = [];

        /// <summary>
        /// Gets a read-only collection of redemptions made by the user.
        /// </summary>
        public virtual IReadOnlyCollection<Redemption> Redemptions => _redemptions.AsReadOnly();

        private ReferralCode? _referralCode;

        /// <summary>
        /// Gets or sets the referral code associated with the user.
        /// </summary>
        public virtual ReferralCode ReferralCode
        {
            set => _referralCode = value;
            get => _referralCode ?? throw new InvalidOperationException("Uninitialized property " + nameof(ReferralCode));
        }

        /// <summary>
        /// Gets the collection of redeemed referrals by the user.
        /// </summary>
        public virtual ICollection<Redemption> RedeemedReferrals { get; private set; } = [];

        /// <summary>
        /// Gets the date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="authId">The authentication ID of the user.</param>
        public User(string authId)
        {
            AuthId = authId;
        }

        /// <summary>
        /// Adds a new referral made by the user.
        /// </summary>
        /// <param name="referralCodeId">The unique identifier of the referral code.</param>
        /// <param name="referralSourceId">The unique identifier of the referral source.</param>
        /// <returns>The newly created referral.</returns>
        public Referral AddReferral(Guid referralCodeId, int referralSourceId)
        {
            var referral = new Referral(UserId, referralCodeId, referralSourceId);
            _referrals.Add(referral);
            return referral;
        }

        /// <summary>
        /// Adds a new redemption made by the user.
        /// </summary>
        /// <param name="referrerId">The unique identifier of the referrer.</param>
        /// <param name="referralCodeId">The unique identifier of the referral code.</param>
        /// <param name="referralSourceId">The unique identifier of the referral source.</param>
        /// <returns>The newly created redemption.</returns>
        public Redemption AddRedemption(Guid referrerId, Guid referralCodeId, int referralSourceId)
        {
            var redemption = new Redemption(UserId, referrerId, referralCodeId, referralSourceId);
            _redemptions.Add(redemption);
            return redemption;
        }
    }
}