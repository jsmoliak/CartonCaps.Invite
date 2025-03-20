using CartonCaps.Invite.API.Infrastructure;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Dto
{
    /// <summary>
    /// Data transfer object for redemption responses.
    /// </summary>
    public class RedemptionResponse
    {
        /// <summary>
        /// Gets the redemption ID.
        /// </summary>
        [Required(ErrorMessage = "RedemptionId is required.")]
        public Guid RedemptionId { get; private set; }

        /// <summary>
        /// Gets the referral code.
        /// </summary>
        [Required(ErrorMessage = "ReferralCode is required.")]
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
        /// Gets the redemption timestamp.
        /// </summary>
        [Required(ErrorMessage = "RedeemedAt is required.")]
        public DateTime RedeemedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedemptionResponse"/> class.
        /// </summary>
        /// <param name="redemptionId">The redemption ID.</param>
        /// <param name="referralCode">The referral code.</param>
        /// <param name="referralSource">The referral source.</param>
        /// <param name="redeemedAt">The redemption timestamp.</param>
        public RedemptionResponse(Guid redemptionId, string referralCode, ReferralSourceEnum referralSource, DateTime redeemedAt)
        {
            RedemptionId = redemptionId;
            ReferralCode = referralCode;
            ReferralSource = referralSource;
            RedeemedAt = redeemedAt;
        }

        /// <summary>
        /// Creates a <see cref="RedemptionResponse"/> from a <see cref="Redemption"/> model.
        /// </summary>
        /// <param name="redemption">The <see cref="Redemption"/> model.</param>
        /// <returns>A <see cref="RedemptionResponse"/> DTO.</returns>
        public static RedemptionResponse FromModel(Redemption redemption)
        {
            return new RedemptionResponse(
                redemption.RedemptionId,
                redemption.ReferralCode.Code,
                Enum.Parse<ReferralSourceEnum>(redemption.ReferralSource.SourceName),
                redemption.RedeemedAt
            );
        }
    }
}