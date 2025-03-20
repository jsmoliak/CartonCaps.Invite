using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartonCaps.Invite.API.Dto
{
    /// <summary>
    /// Data transfer object for referral responses.
    /// </summary>
    public class ReferralResponse
    {
        /// <summary>
        /// Gets the referral ID.
        /// </summary>
        [Required(ErrorMessage = "ReferralId is required.")]
        public Guid ReferralId { get; private set; }

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
        [JsonConverter(typeof(Infrastructure.DtoJsonStringEnumConverter<ReferralSourceEnum>))]
        public ReferralSourceEnum ReferralSource { get; private set; }

        /// <summary>
        /// Gets the creation timestamp.
        /// </summary>
        [Required(ErrorMessage = "CreatedAt is required.")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralResponseDto"/> class.
        /// </summary>
        /// <param name="referralId">The referral ID.</param>
        /// <param name="referralCode">The referral code.</param>
        /// <param name="referralSource">The referral source.</param>
        /// <param name="createdAt">The creation timestamp.</param>
        public ReferralResponse(Guid referralId, string referralCode, ReferralSourceEnum referralSource, DateTime createdAt)
        {
            ReferralId = referralId;
            ReferralCode = referralCode;
            ReferralSource = referralSource;
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Creates a <see cref="ReferralResponseDto"/> from a <see cref="Referral"/> model.
        /// </summary>
        /// <param name="referral">The referral model.</param>
        /// <returns>A <see cref="ReferralResponseDto"/> instance.</returns>
        public static ReferralResponse FromModel(Referral referral)
        {
            return new ReferralResponse
            (
                referral.ReferralId,
                referral.ReferralCode.Code,
                Enum.Parse<ReferralSourceEnum>(referral.ReferralSource.SourceName),
                referral.CreatedAt
            );
        }
    }
}