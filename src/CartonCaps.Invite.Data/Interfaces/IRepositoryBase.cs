using System.Linq.Expressions;

namespace CartonCaps.Invite.Data.Interfaces
{
    /// <summary>
    /// Defines a contract for a generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public interface IRepositoryBase<TEntity>
    {
        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Adds an entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity, or null if not found.</returns>
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
        Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Finds entities that match the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that match the predicate.</returns>
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the first entity that matches the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the predicate, or null if not found.</returns>
        Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Saves changes to the data store asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the underlying database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}