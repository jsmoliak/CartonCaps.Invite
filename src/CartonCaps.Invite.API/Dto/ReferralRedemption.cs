using CartonCaps.Invite.API.Infrastructure;
using CartonCaps.Invite.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Dto
{
    /// <summary>
    /// Data transfer object for redeemed referral information.
    /// </summary>
    public class ReferralRedemption
    {
        /// <summary>
        /// Gets the first name of the referred user.
        /// </summary>
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last initial of the referred user.
        /// </summary>
        [Required(ErrorMessage = "LastInitial is required.")]
        public string LastInitial { get; private set; }

        /// <summary>
        /// Gets the referral code.
        /// </summary>
        [Required(ErrorMessage = "ReferralCode is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "ReferralCode must be 6 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "ReferralCode must be alphanumeric.")]
        public string ReferralCode { get; private set; }

        /// <summary>
        /// Gets the referral status.
        /// </summary>
        [Required(ErrorMessage = "ReferralStatus is required.")]
        [JsonConverter(typeof(DtoJsonStringEnumConverter<ReferralStatusEnum>))]
        public ReferralStatusEnum ReferralStatus { get; private set; }

        /// <summary>
        /// Gets the redemption timestamp.
        /// </summary>
        [Required(ErrorMessage = "RedeemedAt is required.")]
        public DateTime RedeemedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRedemption"/> class.
        /// </summary>
        /// <param name="firstName">The first name of the referred user.</param>
        /// <param name="lastInitial">The last initial of the referred user.</param>
        /// <param name="referralCode">The referral code.</param>
        /// <param name="referralStatus">The referral status.</param>
        /// <param name="redeemedAt">The redemption timestamp.</param>
        public ReferralRedemption(string firstName, string lastInitial, string referralCode, ReferralStatusEnum referralStatus, DateTime redeemedAt)
        {
            FirstName = firstName;
            LastInitial = lastInitial;
            ReferralCode = referralCode;
            ReferralStatus = referralStatus;
            RedeemedAt = redeemedAt;
        }

        /// <summary>
        /// Creates a <see cref="ReferralRedemption"/> from a <see cref="RedeemedReferral"/> model.
        /// </summary>
        /// <param name="redeemedReferral">The redeemed referral model.</param>
        /// <returns>A <see cref="ReferralRedemption"/> instance.</returns>
        public static ReferralRedemption FromModel(RedeemedReferral redeemedReferral)
        {
            return new ReferralRedemption
            (
                redeemedReferral.FirstName,
                redeemedReferral.LastName[0].ToString(),
                redeemedReferral.ReferralCode,
                ReferralStatusEnum.Complete,
                redeemedReferral.RedeemedAt
            );
        }
    }
}