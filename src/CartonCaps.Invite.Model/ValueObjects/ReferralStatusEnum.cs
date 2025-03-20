using System.Text.Json.Serialization;

namespace CartonCaps.Invite.Model.ValueObjects
{
    /// <summary>
    /// Represents the status of a referral as an enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReferralStatusEnum
    {
        /// <summary>
        /// Referral is complete.
        /// </summary>
        Complete = 1
    }
}