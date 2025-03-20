using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;

namespace CartonCaps.Invite.Data.Repositories
{
    /// <summary>
    /// Repository for managing Redemption entities.
    /// </summary>
    public class RedemptionsRepository : RepositoryBase<Redemption>, IRedemptionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedemptionsRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RedemptionsRepository(InviteContext context) : base(context)
        {
        }
    }
}