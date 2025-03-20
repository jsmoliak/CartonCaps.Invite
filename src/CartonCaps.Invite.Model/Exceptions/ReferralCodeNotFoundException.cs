namespace CartonCaps.Invite.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when a referral code is not found.
    /// </summary>
    public class ReferralCodeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralCodeNotFoundException"/> class with a default error message.
        /// </summary>
        public ReferralCodeNotFoundException() : base("Referral code not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralCodeNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ReferralCodeNotFoundException(string message) : base(message) { }
    }
}