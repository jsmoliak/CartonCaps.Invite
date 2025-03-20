using CartonCaps.Invite.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartonCaps.Invite.Data.EntityTypeConfigurations
{
    /// <summary>
    /// Entity type configuration for the Redemption entity.
    /// </summary>
    public class RedemptionEntityConfiguration : IEntityTypeConfiguration<Redemption>
    {
        /// <summary>
        /// Configures the entity type builder for the Redemption entity.
        /// </summary>
        /// <param name="builder">The entity type builder to configure.</param>
        public void Configure(EntityTypeBuilder<Redemption> builder)
        {
            builder.HasOne(r => r.ReferralCode)
                .WithMany(rc => rc.Redemptions)
                .HasForeignKey(r => r.ReferralCodeId);

            builder
                .HasIndex(r => new { r.RedeemerId, r.ReferralCodeId })
                .IsUnique();
        }
    }
}
