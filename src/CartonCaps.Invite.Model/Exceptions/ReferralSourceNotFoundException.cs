namespace CartonCaps.Invite.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when a referral source is not found.
    /// </summary>
    public class ReferralSourceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSourceNotFoundException"/> class with a default error message.
        /// </summary>
        public ReferralSourceNotFoundException() : base("Referral source not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSourceNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ReferralSourceNotFoundException(string message) : base(message) { }
    }
}