using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Invite.Model.ValueObjects
{
    /// <summary>
    /// Represents a user profile value object.
    /// </summary>
    public record UserProfile
    {
        /// <summary>
        /// Gets or sets the authentication ID of the user.
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the referral code associated with the user.
        /// </summary>
        [StringLength(6, MinimumLength = 6, ErrorMessage = "ReferralCode must be 6 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "ReferralCode must be alphanumeric.")]
        public string ReferralCode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> record.
        /// </summary>
        /// <param name="authId">The authentication ID of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="referralCode">The referral code associated with the user.</param>
        public UserProfile(string authId, string firstName, string lastName, string referralCode)
        {
            AuthId = authId;
            FirstName = firstName;
            LastName = lastName;
            ReferralCode = referralCode;
        }
    }
}