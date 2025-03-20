using CartonCaps.Invite.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartonCaps.Invite.Data.EntityTypeConfigurations
{
    /// <summary>
    /// Entity type configuration for the User entity.
    /// </summary>
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the entity type builder for the User entity.
        /// </summary>
        /// <param name="builder">The entity type builder to configure.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.Referrals)
                .WithOne(r => r.Referrer)
                .HasForeignKey(r => r.ReferrerId);

            builder
                .HasMany(u => u.Redemptions)
                .WithOne(r => r.Redeemer)
                .HasForeignKey(r => r.RedeemerId)
                .IsRequired(false);

            builder
                .HasMany(u => u.RedeemedReferrals)
                .WithOne(rr => rr.Referrer)
                .HasForeignKey(r => r.ReferrerId)
                .IsRequired(false);

            builder
                .HasOne(u => u.ReferralCode)
                .WithOne(r => r.User)
                .HasForeignKey<ReferralCode>(r => r.UserId);

        }
    }
}
