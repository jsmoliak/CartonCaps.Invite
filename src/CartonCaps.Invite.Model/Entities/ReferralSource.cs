namespace CartonCaps.Invite.Model.Entities
{
    /// <summary>
    /// Represents the source of a referral.
    /// </summary>
    public class ReferralSource
    {
        /// <summary>
        /// Gets the unique identifier of the referral source.
        /// </summary>
        public int ReferralSourceId { get; private set; }

        /// <summary>
        /// Gets the name of the referral source.
        /// </summary>
        public string SourceName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSource"/> class.
        /// </summary>
        /// <param name="referralSourceId">The unique identifier of the referral source.</param>
        /// <param name="sourceName">The name of the referral source.</param>
        public ReferralSource(int referralSourceId, string sourceName)
        {
            ReferralSourceId = referralSourceId;
            SourceName = sourceName;
        }
    }
}