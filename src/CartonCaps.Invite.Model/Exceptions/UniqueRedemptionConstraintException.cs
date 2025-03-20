namespace CartonCaps.Invite.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when a unique redemption constraint is violated.
    /// </summary>
    public class UniqueRedemptionConstraintException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueRedemptionConstraintException"/> class with a default error message.
        /// </summary>
        public UniqueRedemptionConstraintException() : base("A redemption with the same RedeemerId and ReferralCodeId already exists.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueRedemptionConstraintException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UniqueRedemptionConstraintException(string message) : base(message) { }
    }
}