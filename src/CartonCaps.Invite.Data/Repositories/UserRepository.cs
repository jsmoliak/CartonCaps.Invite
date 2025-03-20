using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Invite.Data.Repositories
{
    /// <summary>
    /// Repository for managing User entities.
    /// </summary>
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UsersRepository(InviteContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a User entity by its authentication ID asynchronously.
        /// </summary>
        /// <param name="authId">The authentication ID of the User.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the User entity, or null if not found.</returns>
        public async Task<User?> GetByAuthIdAsync(string authId)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.AuthId == authId);
        }

        /// <summary>
        /// Retrieves a User entity by its referral code asynchronously.
        /// </summary>
        /// <param name="referralCode">The referral code of the User.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the User entity, or null if not found.</returns>
        public async Task<User?> GetByReferralCodeAsync(string referralCode)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.ReferralCode.Code == referralCode);
        }
    }
}