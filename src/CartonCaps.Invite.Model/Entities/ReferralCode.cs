using System.Text.RegularExpressions;

namespace CartonCaps.Invite.Model.Entities
{
    /// <summary>
    /// Represents a referral code.
    /// </summary>
    public class ReferralCode
    {
        /// <summary>
        /// Gets the unique identifier of the referral code.
        /// </summary>
        public Guid ReferralCodeId { get; private set; }

        private string _code = string.Empty;

        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        public string Code
        {
            get => _code;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Referral code cannot be null or empty.", nameof(value));
                }

                if (value.Length != 6)
                {
                    throw new ArgumentException("Referral code must be 6 characters long.", nameof(value));
                }

                if (!Regex.IsMatch(value, "^[a-zA-Z0-9]+$"))
                {
                    throw new ArgumentException("Referral code must be alphanumeric.", nameof(value));
                }

                _code = value;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the user associated with the referral code.
        /// </summary>
        public Guid UserId { get; private set; }

        private User? _user;

        /// <summary>
        /// Gets or sets the user associated with the referral code.
        /// </summary>
        public virtual User User
        {
            set => _user = value;
            get => _user ?? throw new InvalidOperationException("Uninitialized property " + nameof(User));
        }

        /// <summary>
        /// Gets the collection of redemptions associated with the referral code.
        /// </summary>
        public virtual ICollection<Redemption> Redemptions { get; private set; } = [];

        /// <summary>
        /// Gets the date and time when the referral code was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralCode"/> class.
        /// </summary>
        /// <param name="code">The referral code value.</param>
        /// <param name="userId">The unique identifier of the associated user.</param>
        public ReferralCode(string code, Guid userId)
        {
            Code = code;
            UserId = userId;
        }
    }
}