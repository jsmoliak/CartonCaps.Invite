using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Dto
{
    /// <summary>
    /// Data transfer object for referral requests.
    /// </summary>
    public class ReferralRequest
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
        [JsonRequired]
        [JsonConverter(typeof(Infrastructure.DtoJsonStringEnumConverter<ReferralSourceEnum>))]
        public ReferralSourceEnum ReferralSource { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRequest"/> class.
        /// </summary>
        /// <param name="referralCode">The referral code.</param>
        /// <param name="referralSource">The referral source.</param>
        public ReferralRequest(string referralCode, ReferralSourceEnum referralSource)
        {
            ReferralCode = referralCode;
            ReferralSource = referralSource;
        }

        /// <summary>
        /// Creates a <see cref="ReferralRequest"/> from a <see cref="Referral"/> model.
        /// </summary>
        /// <param name="referral">The referral model.</param>
        /// <returns>A <see cref="ReferralRequest"/> instance.</returns>
        public static ReferralRequest FromModel(Referral referral)
        {
            return new ReferralRequest
            (
                referral.ReferralCode.Code,
                Enum.Parse<ReferralSourceEnum>(referral.ReferralSource.SourceName)
            );
        }
    }
}