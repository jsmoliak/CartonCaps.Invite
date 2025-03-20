using System.Text.Json.Serialization;

namespace CartonCaps.Invite.Model.ValueObjects
{
    /// <summary>
    /// Represents the source of a referral as an enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReferralSourceEnum
    {
        /// <summary>
        /// Referral source is Android.
        /// </summary>
        Android = 1,

        /// <summary>
        /// Referral source is iOS.
        /// </summary>
        iOS,

        /// <summary>
        /// Referral source is Chrome.
        /// </summary>
        Chrome,

        /// <summary>
        /// Referral source is Edge.
        /// </summary>
        Edge,

        /// <summary>
        /// Referral source is Firefox.
        /// </summary>
        Firefox
    }
}