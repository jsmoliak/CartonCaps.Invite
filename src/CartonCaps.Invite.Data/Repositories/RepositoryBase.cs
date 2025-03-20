using CartonCaps.Invite.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CartonCaps.Invite.Data.Repositories
{
    /// <summary>
    /// Base repository class for managing entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected readonly InviteContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RepositoryBase(InviteContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Adds an entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity, or null if not found.</returns>
        public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id, cancellationToken], cancellationToken);
        }

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
        public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Finds entities that match the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that match the predicate.</returns>
        public virtual async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Finds the first entity that matches the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the predicate, or null if not found.</returns>
        public virtual async Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Saves changes to the data store asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the underlying database.</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}