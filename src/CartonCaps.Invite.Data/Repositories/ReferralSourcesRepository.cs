using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Invite.Data.Repositories
{
    /// <summary>
    /// Repository for managing ReferralSource entities.
    /// </summary>
    public class ReferralSourcesRepository : RepositoryBase<ReferralSource>, IReferralSourcesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSourcesRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReferralSourcesRepository(InviteContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a ReferralSource entity by its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the ReferralSource.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the ReferralSource entity.</returns>
        /// <exception cref="ReferralSourceNotFoundException">Thrown when the referral source is not found.</exception>
        public async Task<ReferralSource> GetByNameAsync(string name)
        {
            var referralSource = await _dbContext.ReferralSource.SingleOrDefaultAsync(rs => rs.SourceName == name);
            if (referralSource is null)
                throw new ReferralSourceNotFoundException();
            return referralSource;
        }
    }
}