using CartonCaps.Invite.API.Infrastructure;
using CartonCaps.Invite.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Dto

{
    /// <summary>
    /// Data transfer object for redemption requests.
    /// </summary>
    public class RedemptionRequest
    {
        /// <summary>
        /// Gets the referral code.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "ReferralCode is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "ReferralCode must be 6 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "ReferralCode must be alphanumeric.")]
        public string ReferralCode { get; private set; }

        /// <summary>
        /// Gets the referral source.
        /// </summary>
        [Required(ErrorMessage = "ReferralSource is required.")]
        [JsonConverter(typeof(DtoJsonStringEnumConverter<ReferralSourceEnum>))]
        public ReferralSourceEnum ReferralSource { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedemptionRequest"/> class.
        /// </summary>
        /// <param name="referralCode">The referral code.</param>
        /// <param name="referralSource">The referral source.</param>
        public RedemptionRequest(string referralCode, ReferralSourceEnum referralSource)
        {
            ReferralCode = referralCode;
            ReferralSource = referralSource;
        }
    }
}