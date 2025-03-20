using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Invite.API.Dto
{
    /// <summary>
    /// Data Transfer Object representing a referral link.
    /// </summary>
    public class ReferralLink
    {
        /// <summary>
        /// Gets the referral link.
        /// </summary>
        [Required(ErrorMessage = "ReferralLink is required.")]
        public string referralLink { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralLink"/> class.
        /// </summary>
        /// <param name="referralLink">The referral link string.</param>
        public ReferralLink(string referralLink)
        {
            this.referralLink = referralLink;
        }
    }
}