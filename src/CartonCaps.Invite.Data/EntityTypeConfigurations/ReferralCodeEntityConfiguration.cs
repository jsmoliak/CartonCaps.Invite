using CartonCaps.Invite.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartonCaps.Invite.Data.EntityTypeConfigurations

{
    /// <summary>
    /// Entity type configuration for the ReferralCode entity.
    /// </summary>
    public class ReferralCodeEntityConfiguration : IEntityTypeConfiguration<ReferralCode>
    {
        /// <summary>
        /// Configures the entity type builder for the ReferralCode entity.
        /// </summary>
        /// <param name="builder">The entity type builder to configure.</param>
        public void Configure(EntityTypeBuilder<ReferralCode> builder)
        {
            builder.HasIndex(r => r.Code).IsUnique();
        }
    }
}
