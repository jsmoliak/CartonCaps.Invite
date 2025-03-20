using CartonCaps.Invite.Data.EntityTypeConfigurations;
using CartonCaps.Invite.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Invite.Data
{
    /// <summary>
    /// Database context for the invite system.
    /// </summary>
    public class InviteContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet of User entities.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Referral entities.
        /// </summary>
        public DbSet<Referral> Referrals { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Redemption entities.
        /// </summary>
        public DbSet<Redemption> Redemptions { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of ReferralCode entities.
        /// </summary>
        public DbSet<ReferralCode> ReferralCodes { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of ReferralSource entities.
        /// </summary>
        public DbSet<ReferralSource> ReferralSource { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public InviteContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context. The resulting model will be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RedemptionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReferralCodeEntityConfiguration());

            modelBuilder.Entity<ReferralSource>().HasData(
                new ReferralSource(1, "Android"),
                new ReferralSource(2, "iOS"),
                new ReferralSource(3, "Chrome"),
                new ReferralSource(4, "Edge"),
                new ReferralSource(5, "Firefox")
            );
        }
    }
}