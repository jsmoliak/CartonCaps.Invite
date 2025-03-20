using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Invite.Model.ValueObjects
{
    /// <summary>
    /// Represents a redeemed referral value object.
    /// </summary>
    public record RedeemedReferral
    {
        /// <summary>
        /// Gets the first name of the redeemed user.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name of the redeemed user.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the referral code used for redemption.
        /// </summary>
        [StringLength(6, MinimumLength = 6, ErrorMessage = "ReferralCode must be 6 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "ReferralCode must be alphanumeric.")]
        public string ReferralCode { get; private set; }

        /// <summary>
        /// Gets the date and time when the referral was redeemed.
        /// </summary>
        public DateTime RedeemedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeemedReferral"/> record.
        /// </summary>
        /// <param name="firstName">The first name of the redeemed user.</param>
        /// <param name="lastName">The last name of the redeemed user.</param>
        /// <param name="referralCode">The referral code used for redemption.</param>
        /// <param name="redeemedAt">The date and time when the referral was redeemed.</param>
        public RedeemedReferral(string firstName, string lastName, string referralCode, DateTime redeemedAt)
        {
            FirstName = firstName;
            LastName = lastName;
            ReferralCode = referralCode;
            RedeemedAt = redeemedAt;
        }
    }
}