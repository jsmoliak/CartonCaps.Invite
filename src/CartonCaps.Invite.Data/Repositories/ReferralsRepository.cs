using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;

namespace CartonCaps.Invite.Data.Repositories
{
    /// <summary>
    /// Repository for managing Referral entities.
    /// </summary>
    public class ReferralsRepository : RepositoryBase<Referral>, IReferralsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralsRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReferralsRepository(InviteContext context) : base(context)
        {
        }
    }
}